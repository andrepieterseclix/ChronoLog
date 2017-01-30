namespace CLog.UI.Framework.Testing.Models
{
    public class MockModel : TestParameterModelBase
    {
        public MockModel(string text, object obj, params MethodModel[] methods)
            : base(text)
        {
            foreach (MethodModel methodModel in methods)
            {
                Children.Add(methodModel);
            }
        }

        #region Properties

        public object Object { get; private set; }

        #endregion
    }
}
