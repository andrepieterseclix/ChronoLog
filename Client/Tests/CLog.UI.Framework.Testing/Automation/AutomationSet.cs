using System;
using System.Collections.Generic;

namespace CLog.UI.Framework.Testing.Automation
{
    public class AutomationSet
    {
        #region Fields

        private readonly Queue<AutomationAction> _actionQueue = new Queue<AutomationAction>();

        private readonly Action _callback;

        #endregion

        #region Constructors

        public AutomationSet(Action callback = null)
        {
            _callback = callback;
        }

        #endregion

        #region Methods

        public void Enqueue(AutomationAction action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            _actionQueue.Enqueue(action);
        }

        public void Run()
        {
            try
            {
                AutomationAction action = null;

                while (_actionQueue.Count > 0)
                {
                    action = _actionQueue.Dequeue();
                    action.Invoke();
                }
            }
            finally
            {
                _callback?.Invoke();
            }
        }

        #endregion
    }
}
