using CLog.ServiceClients.Security;
using System;
using System.Globalization;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Threading;

namespace CLog.ServiceClients.MessageInspectors
{
    /// <summary>
    /// Represents the message inspector that will add the session information linked to a user's active session on the server.
    /// </summary>
    /// <seealso cref="System.ServiceModel.Dispatcher.IClientMessageInspector" />
    public class ClientMessageSessionInspector : IClientMessageInspector
    {
        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
        }

        /// <summary>
        /// Enables inspection or modification of a message before a request message is sent to a service.
        /// </summary>
        /// <param name="request">The message to be sent to the service.</param>
        /// <param name="channel">The WCF client object channel.</param>
        /// <returns>
        /// The object that is returned as the <paramref name="correlationState " />argument of the <see cref="M:System.ServiceModel.Dispatcher.IClientMessageInspector.AfterReceiveReply(System.ServiceModel.Channels.Message@,System.Object)" /> method. This is null if no correlation state is used.The best practice is to make this a <see cref="T:System.Guid" /> to ensure that no two <paramref name="correlationState" /> objects are the same.
        /// </returns>
        /// <exception cref="System.ApplicationException">
        /// The application's thread principal has not been configured correctly.
        /// or
        /// The application's identity has not been set correctly.
        /// </exception>
        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            ClientPrincipal principal = Thread.CurrentPrincipal as ClientPrincipal;

            if (principal == null)
                throw new ApplicationException("The application's thread principal has not been configured correctly.");

            ClientIdentity identity = principal.Identity;
            if (identity == null)
                throw new ApplicationException("The application's identity has not been set correctly.");

            if (!(identity is AnonymousClientIdentity))
            {
                HttpRequestMessageProperty requestMessageProperty = new HttpRequestMessageProperty();
                string cookie = string.Format(CultureInfo.CurrentCulture, "{0}/{1}/{2}", identity.UserName, identity.SessionId, identity.SessionKey);
                requestMessageProperty.Headers[HttpResponseHeader.SetCookie] = cookie;
                request.Properties[HttpRequestMessageProperty.Name] = requestMessageProperty;
            }

            return null;
        }
    }
}
