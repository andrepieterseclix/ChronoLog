using System;
using System.Collections.Generic;

namespace CLog.UI.Framework.Testing.Helpers
{
    public static class ReflectionHelper
    {
        public static Type[] GetTypesAssignableFrom(Type type, string[] assemblyFiles)
        {
            List<Type> typeList = new List<Type>();
            AppDomain domain = AppDomain.CreateDomain("TempDomain");

            ReflectionHelperMarshalByRef obj =
                (ReflectionHelperMarshalByRef)domain.CreateInstanceAndUnwrap(typeof(ReflectionHelperMarshalByRef).Assembly.FullName, typeof(ReflectionHelperMarshalByRef).FullName);
            
            typeList.AddRange(obj.GetTypesAssignableFrom(type, assemblyFiles));
            AppDomain.Unload(domain);

            return typeList.ToArray();
        }
    }
}
