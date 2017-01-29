using CLog.Framework.ServiceClients;
using CLog.Framework.Services.Contracts;
using CLog.ServiceClients.Behaviors;
using System;
using System.ServiceModel;

namespace CLog.ServiceClients.Clients
{
    /// <summary>
    /// Represents the base class for service client factories.
    /// </summary>
    /// <typeparam name="T">The service contract.</typeparam>
    /// <seealso cref="CLog.Framework.ServiceClients.IServiceClientFactory{T}" />
    public abstract class ServiceClientFactory<T> : IServiceClientFactory<T>
        where T : IService
    {
        #region Fields

        private readonly string _endpointConfigurationName;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceClientFactory{T}"/> class.
        /// </summary>
        /// <param name="endpointConfigurationName">Name of the endpoint configuration.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        protected ServiceClientFactory(string endpointConfigurationName)
        {
            if (string.IsNullOrWhiteSpace(endpointConfigurationName))
                throw new ArgumentNullException(nameof(endpointConfigurationName));

            _endpointConfigurationName = endpointConfigurationName;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a client proxy and wraps it in a <see cref="T:CLog.Framework.ServiceClients.IServiceClient`1" /> object that supports <see cref="T:System.IDisposable" />.
        /// </summary>
        /// <returns>
        /// The service client.
        /// </returns>
        public IServiceClient<T> Create()
        {
            ChannelFactory<T> channelFactory = new ChannelFactory<T>(_endpointConfigurationName);

            // Add the message interceptor that will add the client side session info.
            channelFactory.Endpoint.Behaviors.Add(new ClientSecurityInterceptorBehavior());

            return new ServiceClient<T>(channelFactory.CreateChannel());
        }

        #endregion
    }
}
