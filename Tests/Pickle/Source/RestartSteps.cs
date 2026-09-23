using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RimWorks.Pickle;
using Verse;

namespace ArchitectStudio.PickleSteps
{
    /// <summary>
    /// A real restart, in two launches: the first configures Architect Studio and leaves the result on
    /// disk on purpose; the second is a NEW game process, started from that file, and looks at what the
    /// mod rebuilt at startup, on a def database freshly generated from XML. The in-process restart
    /// (<c>Architect Studio starts again from its settings file</c>) cannot reach that boundary.
    ///
    /// The launcher runs the two under one hold of the lock (<c>-Then</c>) with the same profile. What
    /// crosses between them is the settings file, which the mod itself wrote, and a marker beside it:
    /// the name of the writing process, the letters the scenario gave its buildings, and a snapshot of
    /// the whole Architect menu (every building's place, every category's order, label and colour).
    ///
    /// The features are played only where the switch mod <c>nelim.architectstudio.restartpass</c> is
    /// staged: it is an empty mod, and its presence is the condition, through <c>@requires</c>. No other
    /// pass loads it, so no other pass runs a half of a chain.
    ///
    /// If the chain is cut between the two launches, the marker and the backup stay on disk. Any later
    /// run that does not take the marker over puts the player's file back from the backup and removes it.
    /// </summary>
    [PickleSteps]
    public class RestartSteps
    {
        /// <summary>Names this game process. Two launches never share it, which is what lets a marker
        /// prove that a restart happened between them.</summary>
        internal static readonly string ProcessId = Guid.NewGuid().ToString("N");

        internal const string SwitchPackageId = "nelim.architectstudio.restartpass";

        private static readonly TimeSpan MarkerLifetime = TimeSpan.FromHours(12);

        internal static string MarkerPath => SettingsSandbox.SettingsPath + ".restart-marker";

        internal static bool SwitchLoaded => ModLister.AllInstalledMods.Any(m =>
            m.Active && string.Equals(m.PackageIdNonUnique, SwitchPackageId, StringComparison.OrdinalIgnoreCase));

        private static string[] ReadMarker()
        {
            try
            {
                return File.Exists(MarkerPath) ? File.ReadAllLines(MarkerPath) : null;
            }
            catch (IOException)
            {
                return null;
            }
        }

        /// <summary>The marker on disk was written by this very process: the first launch of the chain, at its end.</summary>
        internal static bool KeptByThisProcess()
        {
            var lines = ReadMarker();
            return lines != null && lines.Length > 0 && lines[0] == ProcessId;
        }

        /// <summary>
        /// A marker written by ANOTHER process, recently, in a run that has the switch mod: this is the
        /// second launch, and the sandbox must not wipe what it came here to look at.
        /// </summary>
        internal static bool TakesOverKeptConfiguration()
        {
            if (!SwitchLoaded)
            {
                return false;
            }

            var lines = ReadMarker();
            if (lines == null || lines.Length == 0 || lines[0] == ProcessId)
            {
                return false;
            }

            return DateTime.UtcNow - File.GetLastWriteTimeUtc(MarkerPath) < MarkerLifetime;
        }

        internal static void ClearMarker()
        {
            try
            {
                if (File.Exists(MarkerPath))
                {
                    File.Delete(MarkerPath);
                }
            }
            catch (IOException)
            {
                // The next run tries again.
            }
        }

        /// <summary>
        /// The first step of the first launch. The sandbox has already isolated the settings, unless a
        /// marker left by a chain that was cut made it take that launch for a second one; this puts the
        /// profile back to what it is on a clean start whichever of the two happened.
        /// </summary>
        [Given("Architect Studio starts this restart test from a clean profile")]
        public void StartClean(PickleContext ctx)
        {
            ctx.Require(SwitchLoaded,
                $"the switch mod '{SwitchPackageId}' is not loaded: a restart chain is played only in the pass that stages it");
            ClearMarker();
            SettingsSandbox.IsolateNow();
        }

        /// <summary>
        /// The last step of the first launch. It writes the marker and the snapshot, and from then on the
        /// sandbox leaves the configuration alone at the end of the scenario.
        /// </summary>
        [When("Architect Studio keeps this configuration for the next launch")]
        public void Keep(PickleContext ctx)
        {
            ArchitectStudioMod.Instance.WriteSettings();

            var lines = new List<string> { ProcessId };

            Driver.Aliases aliases = null;
            try
            {
                aliases = ctx.Get<Driver.Aliases>();
            }
            catch (InvalidOperationException)
            {
                // A chain that picked no buildings has no letters to hand over.
            }

            if (aliases != null)
            {
                lines.AddRange(aliases.ByName.Select(p => $"alias\t{p.Key}\t{p.Value.defName}"));
            }

            var snapshot = ModSteps.Snapshot();
            lines.AddRange(snapshot.Buildables.Select(p => $"b\t{Clean(p.Key)}\t{Clean(p.Value)}"));
            lines.AddRange(snapshot.Categories.Select(p => $"c\t{Clean(p.Key)}\t{Clean(p.Value)}"));

            File.WriteAllLines(MarkerPath, lines.ToArray());
            ctx.Attach("kept for the next launch",
                $"process {ProcessId}; {snapshot.Buildables.Count} buildings, {snapshot.Categories.Count} categories, " +
                $"{aliases?.ByName.Count ?? 0} named buildings");
        }

        /// <summary>
        /// The first step of the second launch. It refuses to pass when the marker was written by this
        /// same process, so a memory that merely still holds the configuration cannot pass for a file.
        /// </summary>
        [Given("the configuration kept by the previous launch is what this launch started from")]
        public void TakeOver(PickleContext ctx)
        {
            ctx.Require(SwitchLoaded,
                $"the switch mod '{SwitchPackageId}' is not loaded: a restart chain is played only in the pass that stages it");

            var lines = ReadMarker();
            ctx.Require(lines != null && lines.Length > 0,
                $"no restart marker at {MarkerPath}: the launch before this one kept nothing. " +
                "Play the first launch, then this one, under one hold of the lock (-Then).");
            ctx.Assert(lines[0] != ProcessId,
                "the marker was written by THIS process, so nothing restarted between the two features");

            var aliases = new Driver.Aliases();
            var kept = new ModSteps.MenuSnapshot
            {
                Buildables = new Dictionary<string, string>(),
                Categories = new Dictionary<string, string>()
            };

            foreach (var line in lines.Skip(1))
            {
                var parts = line.Split('\t');
                if (parts.Length != 3)
                {
                    continue;
                }

                switch (parts[0])
                {
                    case "alias":
                        BuildableDef def = DefDatabase<ThingDef>.GetNamedSilentFail(parts[2]);
                        def ??= DefDatabase<TerrainDef>.GetNamedSilentFail(parts[2]);
                        ctx.Require(def != null, $"the previous launch named '{parts[2]}' as '{parts[1]}', and it no longer exists");
                        aliases.ByName[parts[1]] = def;
                        break;
                    case "b":
                        kept.Buildables[parts[1]] = parts[2];
                        break;
                    case "c":
                        kept.Categories[parts[1]] = parts[2];
                        break;
                }
            }

            ctx.Set(aliases);
            ctx.Set(kept);
            ctx.Attach("taken over from the previous launch",
                $"written by process {lines[0]}, read by {ProcessId}; {kept.Buildables.Count} buildings, {kept.Categories.Count} categories");
        }

        /// <summary>
        /// Everything the first launch saw in the Architect menu, seen again by a process that has
        /// rebuilt it from scratch: every building's category and group, every category's order, label
        /// and colour.
        /// </summary>
        [Then("the Architect menu is as it was when the previous launch kept it")]
        public void MenuAsKept(PickleContext ctx)
        {
            var before = ctx.Get<ModSteps.MenuSnapshot>();
            var now = ModSteps.Snapshot();
            var diffs = ModSteps.Diff("building", before.Buildables, now.Buildables)
                .Concat(ModSteps.Diff("category", before.Categories, now.Categories)).Take(20).ToList();
            ctx.Assert(diffs.Count == 0,
                "the Architect menu after a real restart differs from the one that was kept:\n" + string.Join("\n", diffs));
        }

        private static string Clean(string text) =>
            (text ?? string.Empty).Replace("\r", " ").Replace("\n", " ").Replace("\t", " ");
    }
}
