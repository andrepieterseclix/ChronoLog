using CLog.Common.Logging;
using CLog.ServiceClients.Security;
using CLog.UI.Common.Exceptions;
using CLog.UI.Common.Messaging;
using CLog.UI.Common.Messaging.Mediator;
using CLog.UI.Common.Services;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace CLog.UI.Common.ViewModels
{
    /// <summary>
    /// Represents the base class for view models.
    /// </summary>
    /// <seealso cref="CLog.UI.Common.ViewModels.BindableBase" />
    /// <seealso cref="System.IDisposable" />
    [DebuggerNonUserCode]
    public abstract class ViewModelBase : BasicViewModelBase, IDisposable
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelBase" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="statusService">The status service.</param>
        /// <param name="dialogService">The dialog service.</param>
        /// <param name="mouseService">The mouse service.</param>
        /// <exception cref="System.ArgumentNullException">
        /// </exception>
        protected ViewModelBase(ILogger logger, IStatusService statusService, IDialogService dialogService, IMouseService mouseService)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));
            if (statusService == null)
                throw new ArgumentNullException(nameof(statusService));
            if (dialogService == null)
                throw new ArgumentNullException(nameof(dialogService));
            if (mouseService == null)
                throw new ArgumentNullException(nameof(mouseService));

            Logger = logger;
            StatusService = statusService;
            DialogService = dialogService;
            MouseService = mouseService;

            // Register all decorated methods to the Mediator
            Mediator.Instance.Register(this);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the logger.
        /// </summary>
        /// <value>
        /// The logger.
        /// </value>
        protected ILogger Logger { get; private set; }

        /// <summary>
        /// Gets the status service.
        /// </summary>
        /// <value>
        /// The status service.
        /// </value>
        protected IStatusService StatusService { get; private set; }

        /// <summary>
        /// Gets the dialog service.
        /// </summary>
        /// <value>
        /// The dialog service.
        /// </value>
        protected IDialogService DialogService { get; private set; }

        /// <summary>
        /// Gets the mouse service.
        /// </summary>
        /// <value>
        /// The mouse service.
        /// </value>
        protected IMouseService MouseService { get; private set; }

        /// <summary>
        /// Gets the mediator that implements the messaging pattern.
        /// </summary>
        /// <value>
        /// The mediator.
        /// </value>
        public static Mediator Mediator
        {
            get { return Mediator.Instance; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Executes the specified action as an anonymous identity.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="callingMethod">The calling method.</param>
        protected void ExecuteAnonymous(Action action, [CallerMemberName]string callingMethod = null)
        {
            EnsureNotDisposed();
            Stopwatch stopwatch = new Stopwatch();

            try
            {
                stopwatch.Start();
                MouseService.SetWait(true);
                LoggerHelper.Debug(Logger, "Started executing '{0}'", GetQualifiedMethodName(callingMethod));
                StatusService.SetStatus(StatusMessageType.Warning, SplitOnUppercase(callingMethod));

                action?.Invoke();
            }
            catch (OutOfMemoryException)
            {
                throw;
            }
            catch (Exception ex)
            {
                LoggerHelper.Fatal(Logger, ex, "Unhandled Exception occurred in view model:  {0}", GetQualifiedMethodName(callingMethod));

                StatusService.SetStatus(StatusMessageType.Error, "An unhandled exception has occurred, please see the logs for more detail.");
            }
            finally
            {
                stopwatch.Stop();
                MouseService.SetWait(false);

                LoggerHelper.Debug(Logger, "Finished executing '{0}' in {1}", GetQualifiedMethodName(callingMethod), stopwatch.Elapsed);
            }
        }

        /// <summary>
        /// Executes the action.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="callingMethod">The caller.</param>
        /// <exception cref="System.InvalidProgramException">The principal has not been configured correctly!</exception>
        protected void Execute(Action<ClientPrincipal> action, [CallerMemberName]string callingMethod = null)
        {
            EnsureNotDisposed();
            Stopwatch stopwatch = new Stopwatch();

            ClientPrincipal principal = null;

            // Read the principal from the UI thread
            Invoke(() =>
            {
                principal = Thread.CurrentPrincipal as ClientPrincipal;
                if (principal == null)
                    throw new InvalidProgramException("The application's thread principal has not been configured correctly.");
            });

            try
            {
                stopwatch.Start();
                MouseService.SetWait(true);
                LoggerHelper.Debug(Logger, "Started executing '{0}'", GetQualifiedMethodName(callingMethod));
                StatusService.SetStatus(StatusMessageType.Warning, SplitOnUppercase(callingMethod));

                action?.Invoke(principal);
            }
            catch (OutOfMemoryException)
            {
                throw;
            }
            catch (SessionExpiredException)
            {
                Mediator.NotifyColleaguesAsync(MessagingConstants.USER_LOGGED_OUT, principal.Identity);
                principal.Identity = null;
            }
            catch (Exception ex)
            {
                LoggerHelper.Fatal(Logger, ex, "Unhandled Exception occurred in view model:  {0}", GetQualifiedMethodName(callingMethod));

                StatusService.SetStatus(StatusMessageType.Error, "An unhandled exception has occurred, please see the logs for more detail.");
            }
            finally
            {
                stopwatch.Stop();
                MouseService.SetWait(false);

                LoggerHelper.Debug(Logger, "Finished executing '{0}' in {1}", GetQualifiedMethodName(callingMethod), stopwatch.Elapsed);
            }
        }

        /// <summary>
        /// Executes the asynchronous.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="callingMethod">The calling method.</param>
        protected void ExecuteAsync(Action<ClientPrincipal> action, [CallerMemberName]string callingMethod = null)
        {
            EnsureNotDisposed();
            Stopwatch stopwatch = new Stopwatch();

            ClientPrincipal principal = null;

            // Read the principal from the UI thread
            Invoke(() =>
            {
                principal = Thread.CurrentPrincipal as ClientPrincipal;
                if (principal == null)
                    throw new InvalidProgramException("The application's thread principal has not been configured correctly.");
            });

            Task.Factory.StartNew(() =>
            {
                try
                {
                    DialogService.ApplyMainWindowOverlay(true);

                    stopwatch.Start();
                    MouseService.SetWait(true);
                    LoggerHelper.Debug(Logger, "Started executing '{0}'", GetQualifiedMethodName(callingMethod));
                    StatusService.SetStatus(StatusMessageType.Warning, SplitOnUppercase(callingMethod));

                    action?.Invoke(principal);
                }
                catch (SessionExpiredException)
                {
                    Mediator.NotifyColleaguesAsync(MessagingConstants.USER_LOGGED_OUT, principal.Identity);
                    principal.Identity = null;
                }
                catch (OutOfMemoryException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    LoggerHelper.Fatal(Logger, ex, "Unhandled Exception occurred in view model:  {0}", GetQualifiedMethodName(callingMethod));

                    StatusService.SetStatus(StatusMessageType.Error, "An unhandled exception has occurred, please see the logs for more detail.");
                }
                finally
                {
                    stopwatch.Stop();
                    MouseService.SetWait(false);
                    DialogService.ApplyMainWindowOverlay(false);

                    LoggerHelper.Debug(Logger, "Finished executing '{0}' in {1}", GetQualifiedMethodName(callingMethod), stopwatch.Elapsed);
                }
            });
        }

        /// <summary>
        /// Gets the qualified name of the method.
        /// </summary>
        /// <param name="methodName">Name of the method.</param>
        /// <returns>The qualified method name.</returns>
        protected string GetQualifiedMethodName(string methodName)
        {
            return string.Format(CultureInfo.CurrentCulture, "{0}.{1}", GetType().Name, methodName);
        }

        /// <summary>
        /// Placing a space in front of each uppercase character.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>The processed string.</returns>
        protected string SplitOnUppercase(string input)
        {
            return Regex.Replace(input.Replace("_", ""), "([a-z])([A-Z])", "$1 $2");
        }

        #endregion

        #region IDisposable Implementation

        /// <summary>
        /// Keep this private, and create and maintain one for every derived class.
        /// </summary>
        private bool _disposed;

        private void EnsureNotDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().Name);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                // Release managed resources

                // Unregister all decorated methods to the Mediator
                Mediator.Instance.Unregister(this);
            }

            // Release native resources
            // NOTE:  call Dispose(false); in finalizer if this class contains unmanaged resources.

            _disposed = true;
        }

        #endregion
    }
}
