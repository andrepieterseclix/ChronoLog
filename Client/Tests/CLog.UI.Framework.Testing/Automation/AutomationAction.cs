using System;
using System.Threading;
using System.Windows.Automation;

namespace CLog.UI.Framework.Testing.Automation
{
    public class AutomationAction
    {
        #region Constructors

        public AutomationAction(Func<AutomationElement> getElement, Action<AutomationElement> action, int retries = 10)
        {
            if (getElement == null)
                throw new ArgumentNullException(nameof(getElement));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            GetElement = getElement;
            Action = action;
            Retries = retries;
        }

        #endregion

        #region Properties

        public int Retries { get; set; }

        public Func<AutomationElement> GetElement { get; private set; }

        public Action<AutomationElement> Action { get; private set; }

        #endregion

        #region Methods

        internal void Invoke()
        {
            AutomationElement element = GetElement();

            for (int i = 0; i < Retries; i++)
            {
                if (element != null)
                    break;

                Thread.Sleep(500);
                element = GetElement();
            }

            if (element == null)
                throw new ElementNotAvailableException();

            try
            {
                Action(element);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}
