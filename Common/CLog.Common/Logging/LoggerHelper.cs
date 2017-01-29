using System;
using System.Globalization;

namespace CLog.Common.Logging
{
    /// <summary>
    /// Represents a helper class that assists in logging operations.
    /// </summary>
    public static class LoggerHelper
    {
        /// <summary>
        /// Logs the specified information.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="message">The message.</param>
        /// <param name="parameters">The parameters.</param>
        public static void Info(ILogger logger, string message, params object[] parameters)
        {
            if (logger == null)
                Console.WriteLine(message, parameters);
            else
                logger.Info(parameters.Length > 0
                    ? string.Format(CultureInfo.CurrentCulture, message, parameters)
                    : message);
        }

        /// <summary>
        /// Logs the specified error message.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="message">The message.</param>
        /// <param name="parameters">The parameters.</param>
        public static void Error(ILogger logger, string message, params object[] parameters)
        {
            if (logger == null)
                Console.WriteLine(message, parameters);
            else
                logger.Error(parameters.Length > 0
                    ? string.Format(CultureInfo.CurrentCulture, message, parameters)
                    : message);
        }

        /// <summary>
        /// Logs the specified exception.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="ex">The ex.</param>
        /// <param name="message">The message.</param>
        /// <param name="parameters">The parameters.</param>
        public static void Exception(ILogger logger, Exception ex, string message, params object[] parameters)
        {
            if (logger == null)
            {
                Console.WriteLine(message, parameters);
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }

            logger.Exception(
                (parameters.Length > 0
                    ? string.Format(CultureInfo.CurrentCulture, message, parameters)
                    : message),
                ex);
        }

        /// <summary>
        /// Logs the specified Fatal message and exception.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="ex">The exception, can be <code>null</code>.</param>
        /// <param name="message">The message.</param>
        /// <param name="parameters">The parameters.</param>
        public static void Fatal(ILogger logger, Exception ex, string message, params object[] parameters)
        {
            if (logger == null)
            {
                Console.WriteLine(message, parameters);
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }

            logger.Fatal(
                (parameters.Length > 0
                    ? string.Format(CultureInfo.CurrentCulture, message, parameters)
                    : message),
                ex);
        }

        /// <summary>
        /// Logs the specified debug message.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="message">The message.</param>
        /// <param name="parameters">The parameters.</param>
        public static void Debug(ILogger logger, string message, params object[] parameters)
        {
            if (logger == null)
                Console.WriteLine(message, parameters);
            else
                logger.Debug(parameters.Length > 0
                    ? string.Format(CultureInfo.CurrentCulture, message, parameters)
                    : message);
        }

        /// <summary>
        /// Logs the specified warning message.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="message">The message.</param>
        /// <param name="parameters">The parameters.</param>
        public static void Warning(ILogger logger, string message, params object[] parameters)
        {
            if (logger == null)
                Console.WriteLine(message, parameters);
            else
                logger.Warning(parameters.Length > 0
                    ? string.Format(CultureInfo.CurrentCulture, message, parameters)
                    : message);
        }
    }
}
