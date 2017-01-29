using CLog.Common.Logging;
using log4net;
using log4net.Config;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace CLog.Common.Log4NetLogger
{
    /// <summary>
    /// Represents the Log4Net implementation of <see cref="ILogger"/>.
    /// </summary>
    /// <seealso cref="CLog.Common.Logging.ILogger" />
    public class Log4NetLogger : ILogger
    {
        #region Constructors
        
        /// <summary>
        /// Initializes the <see cref="Log4NetLogger"/> class.
        /// </summary>
        static Log4NetLogger()
        {
            XmlConfigurator.Configure();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the log.
        /// </summary>
        /// <returns></returns>
        private ILog GetLog()
        {
            StackTrace stack = new StackTrace();

            // Skip this method and the method that called this method (start at 2)
            for (int i = 2; i < stack.FrameCount; i++)
            {
                StackFrame frame = stack.GetFrame(i);
                Type type = frame.GetMethod().DeclaringType;
                if (type == typeof(LoggerHelper) || type == GetType())
                    continue;

                // Skip compiler-generated classes, for example:  <>c__DisplayClass1
                if (type.IsDefined(typeof(CompilerGeneratedAttribute), true))
                    continue;

                return LogManager.GetLogger(type);
            }

            return LogManager.GetLogger(GetType());
        }

        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Info(string message)
        {
            ILog logger = GetLog();
            if (logger.IsInfoEnabled)
                logger.Info(message);
        }

        /// <summary>
        /// Logs the specified error message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Error(string message)
        {
            ILog logger = GetLog();
            if (logger.IsErrorEnabled)
                logger.Error(message);
        }

        /// <summary>
        /// Logs the specified exception.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="ex">The ex.</param>
        public void Exception(string message, Exception ex)
        {
            ILog logger = GetLog();
            if (logger.IsErrorEnabled)
                logger.Error(message, ex);
        }

        /// <summary>
        /// Logs the specified Fatal message and exception.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="ex">The exception, can be <code>null</code>.</param>
        public void Fatal(string message, Exception ex)
        {
            ILog logger = GetLog();
            if (!logger.IsFatalEnabled)
                return;

            if (ex == null)
                logger.Fatal(message);
            else
                logger.Fatal(message, ex);
        }

        /// <summary>
        /// Logs the specified debug message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Debug(string message)
        {
            ILog logger = GetLog();
            if (logger.IsDebugEnabled)
                logger.Debug(message);
        }

        /// <summary>
        /// Logs the specified warning message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Warning(string message)
        {
            ILog logger = GetLog();
            if (logger.IsWarnEnabled)
                logger.Warn(message);
        }

        #endregion
    }
}
