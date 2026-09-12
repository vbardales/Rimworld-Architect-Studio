// Krafs.Publicizer publicises the reference assembly, which is what lets this mod call
// DesignationCategoryDef.ResolveDesignators, DefDatabase<T>.Remove and
// MainTabWindow_Architect.CacheDesPanels, and write Gizmo.disabled. All four are non-public in
// the real Assembly-CSharp: take the Publicize line out of the csproj and the build stops with
// eight errors on them, so the dependency is real and not declared out of caution.
//
// Publicizer defines the attribute type for us and normally applies it through the SDK's
// generated AssemblyInfo — which this project switches off with GenerateAssemblyInfo=false. The
// type was therefore embedded in the assembly and the waiver itself never applied, with nothing
// to say so: the build stays clean and the patches apply either way.
//
// Read this assembly's own GetCustomAttributesData() to check, never the bytes: the type name is
// in the file whether or not the waiver was applied, so grepping the strings returns a false
// positive. That is the exact shape of the trap.
//
// What this mod does NOT establish is that the missing waiver was breaking anything here.
// Deleting a category created by the mod was seen working in a real save, and that path calls
// CacheDesPanels() outside any try/catch, so RimWorld's Mono let a non-public call through where
// the desktop CLR would have thrown. The waiver is declared because Publicizer means it to be,
// and because a runtime that starts enforcing would take the mod down silently.

[assembly: System.Runtime.CompilerServices.IgnoresAccessChecksTo("Assembly-CSharp")]
