using CLog.Framework.Services.Contracts;

namespace CLog.Framework.ServiceClients
{
    /// <summary>
    /// Represents the contract used to construct and destruct a proxy for remote calls to services.
    /// </summary>
    /// <typeparam name="T">The service contract that this factory will implement.</typeparam>
    public interface IServiceClientFactory<T>
        where T : IService
    {
        /// <summary>
        /// Creates a client proxy and wraps it in a <see cref="IServiceClient{T}"/> object that supports <see cref="System.IDisposable"/>.
        /// </summary>
        /// <returns>The service client.</returns>
        IServiceClient<T> Create();
    }
}
