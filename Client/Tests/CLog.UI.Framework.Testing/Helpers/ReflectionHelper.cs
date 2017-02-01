﻿using CLog.UI.Framework.Testing.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CLog.UI.Framework.Testing.Helpers
{
    /// <summary>
    /// http://stackoverflow.com/questions/33975073/proper-way-to-resolving-assemblies-from-subfolders
    /// </summary>
    public static class ReflectionHelper
    {
        private static HashSet<string> _folders = new HashSet<string>();

        private static bool _subscribed;

        public static ModuleAssemblyModel[] GetTypesAssignableFrom(Type type, Predicate<Type> predicate, params string[] assemblyFiles)
        {
            AppDomain domain = AppDomain.CreateDomain("TempDomain");

            ReflectionHelperMarshalByRef obj =
                (ReflectionHelperMarshalByRef)domain.CreateInstanceAndUnwrap(typeof(ReflectionHelperMarshalByRef).Assembly.FullName, typeof(ReflectionHelperMarshalByRef).FullName);

            ModuleAssemblyModel[] result = obj.GetTypesAssignableFrom(type, assemblyFiles, predicate);

            AppDomain.Unload(domain);

            return result;
        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            // Check if the requested assembly is part of the loaded assemblies
            var loadedAssembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.FullName == args.Name);
            if (loadedAssembly != null)
                return loadedAssembly;

            // This resolver is called when an loaded control tries to load a generated XmlSerializer - We need to discard it.
            // http://connect.microsoft.com/VisualStudio/feedback/details/88566/bindingfailure-an-assembly-failed-to-load-while-using-xmlserialization

            var n = new AssemblyName(args.Name);

            if (n.Name.EndsWith(".xmlserializers", StringComparison.OrdinalIgnoreCase))
                return null;

            // http://stackoverflow.com/questions/4368201/appdomain-currentdomain-assemblyresolve-asking-for-a-appname-resources-assembl

            if (n.Name.EndsWith(".resources", StringComparison.OrdinalIgnoreCase))
                return null;

            string assy = null;

            // Find the corresponding assembly file
            foreach (string dir in _folders)
            {
                assy = new[] { "*.dll", "*.exe" }.SelectMany(g => Directory.EnumerateFiles(dir, g)).FirstOrDefault(f =>
                {
                    try
                    {
                        string assemblyName = AssemblyName.GetAssemblyName(f).Name;
                        return n.Name.Equals(assemblyName, StringComparison.OrdinalIgnoreCase);
                    }
                    catch (BadImageFormatException)
                    {
                        return false; /* Bypass assembly is not a .net exe */
                    }
                    catch (Exception ex)
                    {
                        throw new ApplicationException("Error loading assembly " + f, ex);
                    }
                });

                if (assy != null)
                    return Assembly.LoadFrom(assy);
            }

            throw new ApplicationException("Assembly " + args.Name + " not found");
        }

        public static Type LoadTypeExternal(ModuleAssemblyModel testModuleAssembly)
        {
            _folders.Add(Path.GetDirectoryName(testModuleAssembly.AssemblyPath));

            if (!_subscribed)
            {
                AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
                _subscribed = true;
            }

            AppDomain domain = AppDomain.CreateDomain("TempDomain");

            ReflectionHelperMarshalByRef obj =
                (ReflectionHelperMarshalByRef)domain.CreateInstanceAndUnwrap(typeof(ReflectionHelperMarshalByRef).Assembly.FullName, typeof(ReflectionHelperMarshalByRef).FullName);

            Type type = obj.GetTypeFromAssembly(testModuleAssembly);

            AppDomain.Unload(domain);

            return type;
        }
    }
}
