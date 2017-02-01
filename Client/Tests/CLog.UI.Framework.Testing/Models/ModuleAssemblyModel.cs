using System;

namespace CLog.UI.Framework.Testing.Models
{
    [Serializable]
    public class ModuleAssemblyModel
    {
        public ModuleAssemblyModel(string moduleClassName , string assemblyPath)
        {
            ModuleClassName = moduleClassName;
            AssemblyPath = assemblyPath;
        }

        public string ModuleClassName { get; set; }

        public string AssemblyPath { get; set; }
    }
}
