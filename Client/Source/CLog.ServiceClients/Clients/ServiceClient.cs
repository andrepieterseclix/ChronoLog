using CLog.Framework.ServiceClients;
using CLog.Framework.Services.Contracts;
using System;
using System.ServiceModel;

namespace CLog.ServiceClients.Clients
{
    /// <summary>
    /// Represents the wrapper for the service proxy that supports <see cref="IDisposable"/>.
    /// </summary>
    /// <typeparam name="T">The service contract type.</typeparam>
    /// <seealso cref="CLog.Framework.ServiceClients.IServiceClient{T}" />
    internal class ServiceClient<T> : IServiceClient<T>
        where T : IService
    {
        #region Fields

        private readonly T _proxy;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceClient{T}"/> class.
        /// </summary>
        /// <param name="proxy">The service client proxy.</param>
        public ServiceClient(T proxy)
        {
            if (proxy == null)
                throw new ArgumentNullException(nameof(proxy));
            _proxy = proxy;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the service client proxy.
        /// </summary>
        /// <value>
        /// The proxy.
        /// </value>
        public T Proxy
        {
            get { return _proxy; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            IClientChannel channel = _proxy as IClientChannel;
            //channel?.Close();
            channel?.Abort();
            channel?.Dispose();
        }

        #endregion
    }
}
