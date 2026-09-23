using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using RimWorks.Pickle;
using Verse;

namespace ArchitectStudio.PickleSteps
{
    /// <summary>
    /// Every editor action writes Architect Studio's settings to disk at once, so a scenario run on
    /// the player's own game would otherwise rewrite their Architect menu. Each scenario starts from
    /// an empty configuration, and the player's file is put back afterwards.
    ///
    /// The backup is a file, not a copy in memory: if the game dies in the middle of a scenario, the
    /// next scenario finds the backup still there and restores it instead of overwriting it with
    /// the test configuration.
    /// </summary>
    [PickleSteps]
    public class SettingsSandbox
    {
        /// <summary>Group defs present before the scenario, so the ones it created can be dropped.</summary>
        private static HashSet<string> groupsBefore = new HashSet<string>();

        internal static string SettingsPath
        {
            get
            {
                var mod = ArchitectStudioMod.Instance;
                var method = typeof(LoadedModManager).GetMethod("GetSettingsFilename",
                    BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
                return (string)method.Invoke(null, new object[] { mod.Content.FolderName, mod.GetType().Name });
            }
        }

        private static string BackupPath => SettingsPath + ".pickle-backup";

        [BeforeScenario]
        public void IsolateSettings(PickleContext ctx)
        {
            // The second launch of a restart test has to look at what the first one kept, from a process
            // that STARTED with it. Isolating now would wipe exactly that. The player's own file is still
            // in the backup the first launch made, and is put back when the scenario ends.
            if (RestartSteps.TakesOverKeptConfiguration())
            {
                groupsBefore = new HashSet<string>();
                return;
            }

            IsolateNow();
        }

        /// <summary>
        /// Puts the player's file back if a run left it in the backup, remembers what exists, backs the
        /// file up and starts from an empty configuration. Also what a restart test's first launch asks
        /// for explicitly, so that a marker left by a chain that was cut cannot leak into it.
        /// </summary>
        internal static void IsolateNow()
        {
            if (File.Exists(BackupPath))
            {
                // Left behind by a run that never finished: the backup is the player's real file.
                RestoreFromBackup();
            }

            RestartSteps.ClearMarker();

            groupsBefore = new HashSet<string>(
                DefDatabase<DesignatorDropdownGroupDef>.AllDefsListForReading.Select(g => g.defName));

            ArchitectStudioMod.Instance.WriteSettings();
            File.Copy(SettingsPath, BackupPath, overwrite: false);

            ArchitectStudioReset.All();
        }

        [AfterScenario]
        public void RestoreSettings(PickleContext ctx)
        {
            // The first launch of a restart test leaves its configuration on disk ON PURPOSE, and the
            // player's file stays in the backup until the second launch has looked and put it back.
            if (RestartSteps.KeptByThisProcess())
            {
                return;
            }

            if (File.Exists(BackupPath))
            {
                RestoreFromBackup();
            }

            RestartSteps.ClearMarker();
        }

        private static void RestoreFromBackup()
        {
            // Unwind what the scenario built while the test configuration is still loaded: created
            // categories hand their buildings back before their def goes.
            ArchitectStudioReset.All();

            File.Copy(BackupPath, SettingsPath, overwrite: true);
            File.Delete(BackupPath);

            ReloadFromDisk();
            DropGroupsCreatedByTheScenario();
        }

        /// <summary>
        /// Reads the settings file again and replays the startup pass, which is what a restart does
        /// with it. The restart scenario uses the same path, so it is under test there too.
        /// </summary>
        public static void ReloadFromDisk()
        {
            var mod = ArchitectStudioMod.Instance;
            typeof(Mod).GetField("modSettings", BindingFlags.Instance | BindingFlags.NonPublic)
                .SetValue(mod, null);
            var settings = mod.GetSettings<ArchitectStudioSettings>();
            typeof(ArchitectStudioMod).GetProperty(nameof(ArchitectStudioMod.Settings))
                .SetValue(null, settings);

            // Same sequence as StartupInit.
            CustomCategoryRuntime.EnsureDefs();
            CategoryAppearance.ApplyLabels();
            CategoryRuntime.Apply();
            DropdownRuntime.Apply();
        }

        /// <summary>
        /// A deleted custom group keeps its def until the next restart, and the editor lists every
        /// def. Without this, each run would leave its empty test groups in the player's editor.
        /// </summary>
        private static void DropGroupsCreatedByTheScenario()
        {
            var kept = new HashSet<string>(ArchitectStudioMod.Settings.customGroups.Select(e => e.id));
            var remove = typeof(DefDatabase<DesignatorDropdownGroupDef>).GetMethod("Remove",
                BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);

            foreach (var group in DefDatabase<DesignatorDropdownGroupDef>.AllDefsListForReading.ToList())
            {
                if (group.defName.StartsWith("AS_Group_") && !groupsBefore.Contains(group.defName) &&
                    !kept.Contains(group.defName))
                {
                    remove.Invoke(null, new object[] { group });
                }
            }
        }
    }
}
