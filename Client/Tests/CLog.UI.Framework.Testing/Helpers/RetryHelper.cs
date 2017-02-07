using System;
using System.Threading;

namespace CLog.UI.Framework.Testing.Helpers
{
    /// <summary>
    /// NOT PRODUCTION READY!!!
    /// </summary>
    public static class RetryHelper
    {
        /// <summary>
        /// DO NOT USE IN PRODUCTION CODE!!!
        /// </summary>
        /// <param name="retries">The retries.</param>
        /// <param name="pauseBetweenRetriesMilliseconds">The pause between retries in milliseconds.</param>
        /// <param name="action">The action.</param>
        public static void Retry(int retries, int pauseBetweenRetriesMilliseconds, Action action)
        {
            for (int retry = 0; ; retry++)
            {
                try
                {
                    action?.Invoke();
                    break;
                }
                catch (Exception)
                {
                    if (retry < retries)
                    {
                        Thread.Sleep(pauseBetweenRetriesMilliseconds);
                        continue;
                    }

                    throw;
                }
            }
        }
    }
}
