using CLog.Common.Logging;
using CLog.UI.Common.Services;
using CLog.UI.Common.ViewModels;
using CLog.UI.Framework.Testing.Models;
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

        private ModuleAssemblyModel _selectedType;

        #endregion

        #region Constructor

        public SelectModuleViewModel(ILogger logger, IStatusService statusService, IDialogService dialogService, IMouseService mouseService, IEnumerable<ModuleAssemblyModel> types)
            : base(logger, statusService, dialogService, mouseService)
        {
            ModuleInstallers = new ObservableCollection<ModuleAssemblyModel>(types);
            SelectedType = ModuleInstallers.FirstOrDefault();

            OkCommand = CreateCommand(p => { }, p => SelectedType != null);
        }

        #endregion

        #region Properties

        public ICommand OkCommand { get; private set; }

        public ObservableCollection<ModuleAssemblyModel> ModuleInstallers { get; private set; }

        public ModuleAssemblyModel SelectedType
        {
            get { return _selectedType; }
            set { SetProperty(ref _selectedType, value); }
        }

        #endregion
    }
}
