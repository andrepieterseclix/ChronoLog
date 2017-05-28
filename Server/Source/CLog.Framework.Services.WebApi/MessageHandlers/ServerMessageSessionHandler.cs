using CLog.Framework.Services.Common.RequestInterception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CLog.Framework.Services.WebApi.MessageHandlers
{
    /// <summary>
    /// Represents the server message session handler.
    /// </summary>
    public sealed class ServerMessageSessionHandler : DelegatingHandler
    {
        #region Fields
        
        private readonly Func<HttpRequestMessage, string> _getActionFromHeader;

        private readonly Func<HttpRequestMessage, string> _getCookie;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerMessageSessionHandler"/> class.
        /// </summary>
        public ServerMessageSessionHandler()
            : this(GetActionFromHeader, GetCookie)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerMessageSessionHandler"/> class.
        /// </summary>
        /// <remarks>
        /// Use this constructor to mock the delegates for testing purposes.</remarks>
        /// <param name="getActionFromHeader">The get action from header.</param>
        /// <param name="getCookie">The get cookie.</param>
        /// <exception cref="System.ArgumentNullException">
        /// </exception>
        public ServerMessageSessionHandler(Func<HttpRequestMessage, string> getActionFromHeader, Func<HttpRequestMessage, string> getCookie)
        {
            if (getActionFromHeader == null)
                throw new ArgumentNullException(nameof(_getActionFromHeader));
            if (getCookie == null)
                throw new ArgumentNullException(nameof(getCookie));

            _getActionFromHeader = getActionFromHeader;
            _getCookie = getCookie;
        }

        #endregion

        private static string GetActionFromHeader(HttpRequestMessage request)
        {
            return request.RequestUri.LocalPath;
        }

        private static string GetCookie(HttpRequestMessage request)
        {
            IEnumerable<string> values = null;

            return (request.Headers.TryGetValues("Set-Cookie", out values))
                ? values.FirstOrDefault()
                : null;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpMessageSessionInterceptionHandler.SetThreadPrincipal(
                _getActionFromHeader(request),
                _getCookie(request));

            return base.SendAsync(request, cancellationToken);
        }
    }
}
