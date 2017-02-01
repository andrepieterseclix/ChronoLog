using CLog.UI.Framework.Testing.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CLog.UI.Framework.Testing.Helpers
{
    public class ReflectionHelperMarshalByRef : MarshalByRefObject
    {
        public Type[] GetTypesAssignableFrom(Type type, string[] assemblyFiles, Predicate<Type> predicate, Action<string> foundCandidateAssemblyCallback)
        {
            List<Type> typeList = new List<Type>();

            foreach (string assemblyFile in assemblyFiles)
            {
                try
                {
                    Assembly assembly = LoadFile(assemblyFile); // works

                    var types = assembly
                        .GetTypes()
                        .Where(t => type.IsAssignableFrom(t) && predicate(t));

                    typeList.AddRange(types);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

            // Returning types to the main domain will raise an exception if that domain can't resolve the libraries!
            typeList
                .Select(x => new Uri(x.Assembly.CodeBase).LocalPath)
                .ToList()
                .ForEach(x => foundCandidateAssemblyCallback?.Invoke(x));

            return typeList.ToArray();
        }

        public ModuleAssemblyModel[] GetTypesAssignableFrom(Type type, string[] assemblyFiles, Predicate<Type> predicate)
        {
            List<ModuleAssemblyModel> result = new List<ModuleAssemblyModel>();

            foreach (string assemblyFile in assemblyFiles)
            {
                Assembly assembly = LoadFile(assemblyFile);

                var types = assembly
                    .GetTypes()
                    .Where(t => type.IsAssignableFrom(t) && predicate(t));

                result.AddRange(types.Select(t => new ModuleAssemblyModel(t.FullName, assemblyFile)));
            }

            return result.ToArray();
        }

        public Assembly LoadFile(string path)
        {
            ValidatePath(path);

            return Assembly.LoadFile(path);
        }

        private void ValidatePath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException(nameof(path));

            if (!File.Exists(path))
                throw new FileNotFoundException(path);
        }

        public Type GetTypeFromAssembly(ModuleAssemblyModel testModuleAssembly)
        {
            Assembly assembly = LoadFile(testModuleAssembly.AssemblyPath);
            Type type = assembly.GetType(testModuleAssembly.ModuleClassName, true);

            return type;
        }
    }
}
