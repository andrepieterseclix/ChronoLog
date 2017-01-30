using CLog.UI.Common.ViewModels;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace CLog.UI.Framework.Testing.Models
{
    public class TestParameterModelBase : BindableBase
    {
        #region Fields

        private string _text;

        #endregion

        #region Constructors

        protected TestParameterModelBase(string text)
        {
            Text = text;
        }

        #endregion

        #region Properties

        [Browsable(false)]
        public string Text
        {
            get { return _text; }
            set { SetProperty(ref _text, value); }
        }

        [Browsable(false)]
        public ObservableCollection<TestParameterModelBase> Children { get; } = new ObservableCollection<TestParameterModelBase>();

        #endregion
    }
}
