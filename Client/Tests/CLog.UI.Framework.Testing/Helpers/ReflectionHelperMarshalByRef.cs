using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CLog.UI.Framework.Testing.Helpers
{
    public class ReflectionHelperMarshalByRef : MarshalByRefObject
    {
        public Type[] GetTypesAssignableFrom(Type type, string[] assemblyFiles)
        {
            List<Type> typeList = new List<Type>();

            foreach (string assemblyFile in assemblyFiles)
            {
                try
                {
                    Assembly assembly = LoadFile(assemblyFile); // works

                    var types = assembly
                        .GetTypes()
                        .Where(t => type.IsAssignableFrom(t));

                    typeList.AddRange(types);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

            return typeList.ToArray();
        }

        public Assembly Load(string path)
        {
            ValidatePath(path);

            return Assembly.Load(path);
        }

        public Assembly LoadFrom(string path)
        {
            ValidatePath(path);

            return Assembly.LoadFrom(path);
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
    }
}
