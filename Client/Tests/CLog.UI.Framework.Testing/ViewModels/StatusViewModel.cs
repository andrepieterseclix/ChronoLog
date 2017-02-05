using CLog.UI.Common.Services;
using CLog.UI.Common.ViewModels;
using System;
using System.Collections.ObjectModel;

namespace CLog.UI.Framework.Testing.ViewModels
{
    public class StatusViewModel : BasicViewModelBase, IStatusService
    {
        public void SetStatus(StatusMessageType messageType, string format, params object[] args)
        {
            string message = (args.Length > 0) ? string.Format(format, args) : format;
            Invoke(() => Messages.Add(new StatusItemViewModel(
                messageType,
                DateTime.Now,
                message)));
        }

        #region Properties

        public ObservableCollection<StatusItemViewModel> Messages { get; } = new ObservableCollection<StatusItemViewModel>();

        #endregion
    }
}
