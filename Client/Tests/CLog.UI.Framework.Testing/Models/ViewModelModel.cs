using CLog.UI.Common.ViewModels;
using System.ComponentModel;

namespace CLog.UI.Framework.Testing.Models
{
    public class ViewModelModel : TestParameterModelBase
    {
        public ViewModelModel(string text, ViewModelBase viewModel)
            : base(text)
        {
            ViewModel = viewModel;
        }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public ViewModelBase ViewModel { get; set; }
    }
}
