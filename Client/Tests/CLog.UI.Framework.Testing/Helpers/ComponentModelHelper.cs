using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace CLog.UI.Framework.Testing.Helpers
{
    /// <summary>
    /// Represents the static class containing helper methods for working with the .Net component model.
    /// </summary>
    public static class ComponentModelHelper
    {
        /// <summary>
        /// Makes all of the classes in the specified assembly expandable on the WinForms property grid.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        public static void MakeObjectsExpandable(Assembly assembly)
        {
            var types = assembly
                .GetTypes()
                .Where(t => t.IsClass);

            foreach (Type type in types)
            {
                if (!Attribute.IsDefined(type, typeof(TypeConverterAttribute)))
                {
                    var attribute = new TypeConverterAttribute(typeof(ExpandableObjectConverter));
                    TypeDescriptor.AddAttributes(type, attribute);
                }
            }
        }
    }
}
