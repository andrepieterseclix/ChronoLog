using CLog.Common.Logging;
using CLog.UI.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace CLog.UI.Framework.Testing.ViewModels
{
    public sealed class SelectModuleViewModel : ViewModelBase
    {
        #region Fields

        private Type _selectedType;

        #endregion

        #region Constructor

        public SelectModuleViewModel(ILogger logger, IEnumerable<Type> types)
            : base(logger)
        {
            ModuleInstallers = new ObservableCollection<Type>(types);
            SelectedType = ModuleInstallers.FirstOrDefault();

            OkCommand = CreateCommand(OkCommand_Execute, OkCommand_CanExecute);
        }

        #endregion

        #region Properties

        public ICommand OkCommand { get; private set; }

        public ObservableCollection<Type> ModuleInstallers { get; private set; }

        public Type SelectedType
        {
            get { return _selectedType; }
            set { SetProperty(ref _selectedType, value); }
        }

        #endregion

        #region Methods

        private void OkCommand_Execute(object parameter)
        {
        }

        private bool OkCommand_CanExecute(object parameter)
        {
            return SelectedType != null;
        }

        #endregion
    }
}
