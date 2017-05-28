using CLog.Framework.Services.Common.RequestInterception;
using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace CLog.Framework.Services.Wcf.MessageInspectors
{
    /// <summary>
    /// Represents the server message session inspector.
    /// </summary>
    public sealed class ServerMessageSessionInspector : IDispatchMessageInspector
    {
        #region Fields

        public const string INTERCEPTOR_ACCESS_MANAGER_INSTANCE_NAME = "InterceptorAccessManagerInstance";

        private const string INVALID_PARAMETERS_MESSAGE = "Invalid request header parameters!";

        private readonly Func<string> _getActionFromHeader;

        private readonly Func<string> _getCookie;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerMessageSessionInspector"/> class.
        /// </summary>
        public ServerMessageSessionInspector()
            : this(GetActionFromHeader, GetCookie)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerMessageSessionInspector"/> class.
        /// </summary>
        /// <remarks>
        /// Use this constructor to mock the delegates for testing purposes.</remarks>
        /// <param name="getActionFromHeader">The get action from header.</param>
        /// <param name="getCookie">The get cookie.</param>
        /// <exception cref="System.ArgumentNullException">
        /// </exception>
        public ServerMessageSessionInspector(Func<string> getActionFromHeader, Func<string> getCookie)
        {
            if (getActionFromHeader == null)
                throw new ArgumentNullException(nameof(_getActionFromHeader));
            if (getCookie == null)
                throw new ArgumentNullException(nameof(getCookie));

            _getActionFromHeader = getActionFromHeader;
            _getCookie = getCookie;
        }

        #endregion

        #region Methods

        private static string GetActionFromHeader()
        {
            return OperationContext.Current.IncomingMessageHeaders.Action;
        }

        private static string GetCookie()
        {
            HttpRequestMessageProperty messageProperty = (HttpRequestMessageProperty)OperationContext.Current.IncomingMessageProperties[HttpRequestMessageProperty.Name];

            return messageProperty.Headers.Get("Set-Cookie");
        }

        /// <summary>
        /// Called after an inbound message has been received but before the message is dispatched to the intended operation.
        /// </summary>
        /// <param name="request">The request message.</param>
        /// <param name="channel">The incoming channel.</param>
        /// <param name="instanceContext">The current service instance.</param>
        /// <returns>
        /// The object used to correlate state. This object is passed back in the <see cref="M:System.ServiceModel.Dispatcher.IDispatchMessageInspector.BeforeSendReply(System.ServiceModel.Channels.Message@,System.Object)" /> method.
        /// </returns>
        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            HttpMessageSessionInterceptionHandler.SetThreadPrincipal(_getActionFromHeader(), _getCookie());

            return null;
        }

        /// <summary>
        /// Called after the operation has returned but before the reply message is sent.
        /// </summary>
        /// <param name="reply">The reply message. This value is null if the operation is one way.</param>
        /// <param name="correlationState">The correlation object returned from the <see cref="M:System.ServiceModel.Dispatcher.IDispatchMessageInspector.AfterReceiveRequest(System.ServiceModel.Channels.Message@,System.ServiceModel.IClientChannel,System.ServiceModel.InstanceContext)" /> method.</param>
        public void BeforeSendReply(ref Message reply, object correlationState)
        {
        }

        #endregion
    }
}
