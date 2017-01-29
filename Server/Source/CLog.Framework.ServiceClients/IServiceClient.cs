using CLog.Framework.Services.Contracts;
using System;

namespace CLog.Framework.ServiceClients
{
    /// <summary>
    /// Represents the contract for a wrapper of service proxies that supports <see cref="IDisposable"/>.
    /// </summary>
    /// <typeparam name="T">The service contract type.</typeparam>
    /// <seealso cref="System.IDisposable" />
    public interface IServiceClient<T> : IDisposable
        where T : IService
    {
        T Proxy { get; }
    }
}
