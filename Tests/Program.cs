using System;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;

internal static class Program
{
    private static int Main(string[] args)
    {
        AssemblyLoadContext.Default.Resolving += (context, name) =>
        {
            string path = Path.Combine(args[0], name.Name + ".dll");
            return File.Exists(path) ? context.LoadFromAssemblyPath(path) : null;
        };
        try
        {
            Assembly.GetExecutingAssembly().GetType("ArchitectStudioTests.BehaviorTests")
                .GetMethod("Run").Invoke(null, new object[] { args[1], args[2] });
            return 0;
        }
        catch (Exception e) { Console.Error.WriteLine(e); return 1; }
    }
}
