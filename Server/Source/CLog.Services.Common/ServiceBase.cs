using CLog.Common.BaseClasses;
using CLog.Common.Logging;
using CLog.Framework.Business.Messages;
using CLog.Framework.Security;
using CLog.Framework.Services.Contracts;
using CLog.Framework.Services.Extensions;
using CLog.Framework.Services.Models;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.Threading;

namespace CLog.Services.Common
{
    /// <summary>
    /// Represents the base class for micro service implementations.
    /// </summary>
    /// <seealso cref="CLog.Common.BaseClasses.CommonBase" />
    /// <seealso cref="CLog.Framework.Services.Contracts.IService" />
    [DebuggerNonUserCode]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, IncludeExceptionDetailInFaults = true)]
    public abstract class ServiceBase : CommonBase, IService
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceBase" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public ServiceBase(ILogger logger)
            : base(logger)
        {
        }

        #endregion

        #region Properties
        
        /// <summary>
        /// Gets the identity for the current user.
        /// </summary>
        /// <value>
        /// The user identity.
        /// </value>
        protected ServerIdentity UserIdentity
        {
            get
            {
                ServerPrincipal principal = Thread.CurrentPrincipal as ServerPrincipal;
                return principal?.Identity;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is anonymous.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is anonymous; otherwise, <c>false</c>.
        /// </value>
        protected bool IsAnonymous
        {
            get { return UserIdentity is AnonymousServerIdentity; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Executes the specified action.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="action">The action.</param>
        /// <param name="callingMethod">The calling method.</param>
        /// <returns>
        /// The response.
        /// </returns>
        public TResponse Execute<TResponse>(Action<TResponse> action, [CallerMemberName]string callingMethod = null)
            where TResponse : ResponseBase, new()
        {
            EnsureNotDisposed();
            TResponse response = new TResponse();

            try
            {
                ServerIdentity identity = UserIdentity;

                if (!IsAnonymous && identity.SessionExpired)
                {
                    response.SessionExpired = identity.SessionExpired;
                    response.Errors.Add(ErrorMessages.SessionExpired().Map());

                    return response;
                }

                action?.Invoke(response);
            }
            catch (OutOfMemoryException)
            {
                throw;
            }
            catch (Exception ex)
            {
                LoggerHelper.Fatal(Logger, ex, "Unhandled Exception occurred in service:  {0}", GetQualifiedMethodName(callingMethod));

                response.Errors.Add(ErrorMessages.UnhandledBusinessException().Map());
            }

            return response;
        }

        #endregion
    }
}
