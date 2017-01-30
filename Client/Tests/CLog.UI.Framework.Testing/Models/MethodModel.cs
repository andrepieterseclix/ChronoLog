using System;
using System.ComponentModel;
using System.Reflection;

namespace CLog.UI.Framework.Testing.Models
{
    public class MethodModel : TestParameterModelBase
    {
        public MethodModel(string text, object returnObject)
            : base(text)
        {
            ReturnObject = returnObject;
        }

        #region Properties

        [DisplayName("Return")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public object ReturnObject { get; set; }

        #endregion
    }
}
