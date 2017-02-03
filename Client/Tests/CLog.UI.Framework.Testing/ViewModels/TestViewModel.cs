using CLog.Common.Logging;
using CLog.UI.Common.Services;
using CLog.UI.Common.ViewModels;
using CLog.UI.Framework.Testing.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace CLog.UI.Framework.Testing.ViewModels
{
    public class TestViewModel : ViewModelBase
    {
        #region Fields

        private object _selectedObject;

        #endregion

        #region Constructors

        public TestViewModel(ILogger logger, IStatusService statusService, IDialogService dialogService, IMouseService mouseService, StatusViewModel status)
            : base(logger, statusService, dialogService, mouseService)
        {
            Status = status;

            PropertyChanged += TestViewModel_PropertyChanged;
        }
        
        #endregion

        #region Properties

        public ObservableCollection<TestParameterModelBase> TestParameterModels { get; } = new ObservableCollection<TestParameterModelBase>();

        public ObservableCollection<MethodRunViewModel> RunMethodModels { get; } = new ObservableCollection<MethodRunViewModel>();

        public StatusViewModel Status { get; private set; }

        public object SelectedObject
        {
            get { return _selectedObject; }
            set { SetProperty(ref _selectedObject, value); }
        }

        #endregion

        #region Event Handlers

        private void TestViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(SelectedObject))
            {
                RunMethodModels.Clear();
                if (SelectedObject == null)
                    return;

                ViewModelModel model = SelectedObject as ViewModelModel;
                if (model == null)
                    return;

                MethodInfo[] methods = model.ViewModel
                    .GetType()
                    .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                    .Where(x => !x.IsSpecialName && x.DeclaringType != typeof(object))
                    .OrderBy(x => x.Name)
                    .ToArray();

                foreach (MethodInfo method in methods)
                    RunMethodModels.Add(new MethodRunViewModel(Logger, StatusService, DialogService, MouseService, model.ViewModel, method));
            }
        }

        #endregion
    }
}
