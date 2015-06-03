using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Web.Models
{
    public static class ConfigurationExtensions
    {
        public static void UpdateApp(this config config)
        {
            var newPath = Path.GetFullPath(config.resourcesDirectory);
            if (WebApiApplication.Config.resourcesDirectory != newPath)
            {
                Trace.WriteLine("Adding assemblies in the directory");
                TypeCache.Cache = GetTypesMap(newPath);
                WebApiApplication.Config.resourcesDirectory = newPath;
            }
        }

        public static string GetProbingPath(this config config)
        {
            if (config != null && !String.IsNullOrWhiteSpace(config.resourcesDirectory))
            {
                if (Directory.Exists(config.resourcesDirectory))
                {
                    return config.resourcesDirectory;
                }
            }

            return Environment.CurrentDirectory;
        }

        public static bool isValidProbingPath(this config config)
        {
            if (config != null && !String.IsNullOrWhiteSpace(config.resourcesDirectory))
            {
                if (Directory.Exists(config.resourcesDirectory))
                {
                    return true;
                }
            }

            return false;
        }

        public static IDictionary<string, Type> GetTypesMap(string path)
        {
            if (!Directory.Exists(path))
            {
                throw new ArgumentException("Non-existant path" + path);
            }

            SortedDictionary<string, Type> dictionary = new SortedDictionary<string, Type>(StringComparer.OrdinalIgnoreCase);

            foreach (Assembly assembly in LoadAllAssemblies(path))
            {
                try
                {
                    var types = assembly.GetTypes()
                                    .Where(t =>
                                    {
                                        return t.GetMethods().Count(m => m.Name == "Execute") > 0;
                                    });

                    foreach (var type in types)
                    {
                        dictionary.Add(type.FullName, type);
                    }
                }
                catch (Exception)
                {
                }
            }

            Trace.WriteLine(JsonConvert.SerializeObject(dictionary, Formatting.Indented));

            return dictionary;
        }

        static Assembly LoadHandler(object sender, ResolveEventArgs args, string directory)
        {
            string folderPath = Path.GetFullPath(directory);
            string assemblyPath = Path.Combine(folderPath, new AssemblyName(args.Name).Name + ".dll");
            if (File.Exists(assemblyPath) == false)
                return null;
            Assembly assembly = Assembly.LoadFrom(assemblyPath);
            return assembly;
        }

        public static IEnumerable<Assembly> LoadAllAssemblies(string path)
        {
            string binPath = System.IO.Path.Combine(path); // note: don't use CurrentEntryAssembly or anything like that.

            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
            {
                return LoadHandler(sender, args, path);
            };

            foreach (string dll in Directory.GetFiles(binPath, "*.dll", SearchOption.AllDirectories))
            {
                Assembly assembly = null;

                try
                {
                    assembly = Assembly.LoadFile(dll);
                }
                catch (FileLoadException loadEx)
                { } // The Assembly has already been loaded.
                catch (BadImageFormatException imgEx)
                { } // If a BadImageFormatException exception is thrown, the file is not an assembly.

                if (assembly != null)
                {
                    yield return assembly;
                }

            } // foreach dll
        }
    }

    static class TypeCache
    {
        public static IDictionary<string, Type> Cache { get; set; }
    }
}