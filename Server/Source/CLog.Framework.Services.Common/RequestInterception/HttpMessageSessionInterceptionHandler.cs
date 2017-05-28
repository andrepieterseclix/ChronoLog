using CLog.Business.Security.Contracts.Access;
using CLog.Common.Logging;
using CLog.Framework.Business.Models.Results;
using CLog.Framework.Security;
using CLog.Models.Access;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Linq;
using System.Security;
using System.Text.RegularExpressions;
using System.Threading;

namespace CLog.Framework.Services.Common.RequestInterception
{
    /// <summary>
    /// Represents the logic used to set the Thread Principal and User Identity.
    /// </summary>
    public static class HttpMessageSessionInterceptionHandler
    {
        #region Fields

        public const string INTERCEPTOR_ACCESS_MANAGER_INSTANCE_NAME = "InterceptorAccessManagerInstance";

        private const string INVALID_PARAMETERS_MESSAGE = "Invalid request header parameters!";

        #endregion

        #region Methods

        public static void SetThreadPrincipal(string action, string cookie)
        {
            ILogger logger = ServiceLocator.Current.GetInstance<ILogger>();
            
            Match shortActionMatch = Regex.Match(action, @"\w+/\w+$");
            string shortAction = shortActionMatch.Success ? shortActionMatch.Value : action;

            LoggerHelper.Debug(logger, "Intercepting '{0}'", action);
            
            if (string.IsNullOrWhiteSpace(cookie))
            {
                // Anonymous
                Thread.CurrentPrincipal = new ServerPrincipal();
                LoggerHelper.Info(logger, "User 'Anonymous' is requesting '{0}'", shortAction);

                return;
            }

            string[] values = cookie.Split('/');

            // Get data from cookie
            if (values.Length != 3)
                throw new SecurityException(INVALID_PARAMETERS_MESSAGE);

            Guid sessionId;

            if (!Guid.TryParse(values[1], out sessionId))
                throw new SecurityException(INVALID_PARAMETERS_MESSAGE);

            string userName = values[0];
            string sessionKey = values[2];

            // Validate the user and session
            BusinessResult<SessionState> result = null;
            using (IAccessManager accessManager = ServiceLocator.Current.GetInstance<IAccessManager>(INTERCEPTOR_ACCESS_MANAGER_INSTANCE_NAME))
            {
                //LoggerHelper.Debug(logger, "Access Manager:  {0}", accessManager.GetHashCode());
                result = accessManager.ValidateSession(userName, sessionId, sessionKey);
            }

            if (result.HasErrors && (!result.Result?.IsExpired ?? true))
            {
                string messageCombined = string.Join("\r\n", result.Errors.Select(x => x.Message));

                throw new SecurityException(messageCombined);
            }

            // Set the thread principal
            ServerIdentity identity = new ServerIdentity(userName, result.Result.UserId, sessionId, sessionKey, result.Result.IsExpired, new string[0]);
            ServerPrincipal principal = new ServerPrincipal(identity);
            Thread.CurrentPrincipal = principal;

            LoggerHelper.Info(logger, "User '{0}', session '{1}' is requesting '{2}'", userName, sessionId, shortAction);
        }

        #endregion
    }
}
