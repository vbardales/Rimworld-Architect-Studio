using System;
using System.Runtime.CompilerServices;
using Verse;

namespace ArchitectStudio
{
    /// <summary>
    /// Performs one deliberate access to a non-public member of the game and says so in the log if
    /// the runtime refuses it. See AccessChecks.cs for what is being guarded: the publicizer's
    /// waiver is applied through a generated file this project switches off, so losing it again
    /// costs one line of csproj and produces a build that is clean, silent and broken.
    ///
    /// This exists because the failure has no other symptom. The patches still apply, startup logs
    /// nothing, and only the paths that touch a non-public member die - here that would be category
    /// creation and deletion, dropdown rebuilds and the research-locked option, each at the moment
    /// it is first used rather than at load.
    /// </summary>
    public static class AccessCheckProbe
    {
        /// <summary>Value read by the probe. Kept so the read has a destination and cannot be elided.</summary>
        public static bool LastProbeResult { get; private set; }

        public static void Run()
        {
            try
            {
                LastProbeResult = ReadNonPublicField();
            }
            catch (Exception ex)
            {
                Log.Error($"[Architect Studio] The runtime refused access to a non-public member of the " +
                          $"game ({ex.GetType().Name}: {ex.Message}). This assembly is missing the " +
                          $"IgnoresAccessChecksTo waiver that Krafs.Publicizer normally applies through " +
                          $"the generated AssemblyInfo, which this project disables. Creating and " +
                          $"deleting categories, rebuilding dropdowns and the research-locked option " +
                          $"will all fail, each on first use. Rebuild with Source/AccessChecks.cs " +
                          $"present; patching the shipped assembly by hand does not survive a build.");
            }
        }

        /// <summary>
        /// Kept in its own method, and never inlined, on purpose. An access check is made when the
        /// method performing the access is compiled, not when the statement runs, so an access
        /// placed directly inside the try above would throw on entering <see cref="Run"/> and sail
        /// straight past its own catch.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool ReadNonPublicField()
        {
            // Gizmo.disabled is protected, and the value is returned rather than dropped so that
            // the read cannot be optimised away in a release build.
            return new Command_Action().disabled;
        }
    }
}
