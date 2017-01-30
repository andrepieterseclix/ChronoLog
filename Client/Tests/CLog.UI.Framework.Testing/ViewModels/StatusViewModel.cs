using CLog.Common.Logging;
using CLog.UI.Common.Services;
using CLog.UI.Common.ViewModels;
using System.Collections.ObjectModel;

namespace CLog.UI.Framework.Testing.ViewModels
{
    public class StatusViewModel : ViewModelBase, IStatusService
    {
        public StatusViewModel(ILogger logger)
            : base(logger)
        {
        }

        public void SetStatus(StatusMessageType messageType, string format, params object[] args)
        {
            string message = (args.Length > 0) ? string.Format(format, args) : format;
            Invoke(() => Messages.Add(string.Format("({0}) - {1}", messageType, message)));
        }

        #region Properties

        public ObservableCollection<string> Messages { get; } = new ObservableCollection<string>();

        #endregion
    }
}
