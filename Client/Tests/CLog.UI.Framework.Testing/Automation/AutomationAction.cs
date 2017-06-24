using System;
using System.Threading;
using System.Windows.Automation;

namespace CLog.UI.Framework.Testing.Automation
{
    public class AutomationAction : IAction
    {
        #region Fields

        public const int SLEEP = 500;

        private Func<AutomationElement> _getElement;

        private Predicate<AutomationElement> _action;

        #endregion

        #region Constructors

        public AutomationAction(Func<AutomationElement> getElement, Predicate<AutomationElement> action, int retries = 10)
        {
            if (getElement == null)
                throw new ArgumentNullException(nameof(getElement));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            _getElement = getElement;
            _action = action;
            Retries = retries;
        }

        #endregion

        #region Properties

        public int Retries { get; set; }
        
        #endregion

        #region Methods

        public void Invoke()
        {
            // Give element some time to load (sleep on first try), UI Automation Framework is finicky :(
            AutomationElement element = null;

            for (int i = 0; i < Retries; i++)
            {
                if (element != null)
                    break;

                Thread.Sleep(SLEEP);
                element = _getElement();
            }

            if (element == null)
                throw new Exception("The automation element could not be obtained.");

            // Execute action
            for (int i = 0; i < Retries; i++)
            {
                if (_action(element))
                    return;

                Thread.Sleep(SLEEP);
            }

            throw new Exception("The automation action could not be executed.");
        }

        #endregion
    }
}
