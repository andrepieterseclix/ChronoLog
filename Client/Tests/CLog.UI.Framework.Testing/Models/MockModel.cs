using System.ComponentModel;

namespace CLog.UI.Framework.Testing.Models
{
    public class MockModel : TestParameterModelBase
    {
        public MockModel(string text, object obj, params MethodModel[] methods)
            : base(text)
        {
            Object = obj;

            foreach (MethodModel methodModel in methods)
            {
                Children.Add(methodModel);
            }
        }

        #region Properties

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public object Object { get; private set; }

        #endregion
    }
}
