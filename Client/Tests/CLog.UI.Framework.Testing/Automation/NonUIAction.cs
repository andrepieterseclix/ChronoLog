using System;
using System.Threading;

namespace CLog.UI.Framework.Testing.Automation
{
    public class NonUIAction : IAction
    {
        #region Fields

        private Action _action;

        #endregion

        #region Constructors

        public NonUIAction(Action action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            _action = action;
        }

        #endregion

        #region Methods

        public void Invoke()
        {
            Thread.Sleep(AutomationAction.SLEEP);

            _action();
        }

        #endregion
    }
}
