using System;
using System.Threading;
using System.Windows.Automation;

namespace CLog.UI.Framework.Testing.Automation
{
    public class AutomationAction
    {
        #region Fields

        private const int SLEEP = 500;

        #endregion

        #region Constructors

        public AutomationAction(Func<AutomationElement> getElement, Predicate<AutomationElement> action, int retries = 10)
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

        public Predicate<AutomationElement> Action { get; private set; }

        #endregion

        #region Methods

        internal void Invoke()
        {
            AutomationElement element = GetElement();

            for (int i = 0; i < Retries; i++)
            {
                if (element != null)
                    break;

                Thread.Sleep(SLEEP);
                element = GetElement();
            }

            if (element == null)
                throw new Exception("The automation element could not be obtained.");

            // Execute action
            for (int i = 0; i < Retries; i++)
            {
                if (Action(element))
                    return;

                Thread.Sleep(SLEEP);
            }

            throw new Exception("The automation action could not be executed.");
        }

        #endregion
    }
}
