using CLog.Framework.Services.Models;
using System.Runtime.Serialization;

namespace CLog.Services.Models.Access.DataTransfer
{
    /// <summary>
    /// Represents the logout response model.
    /// </summary>
    /// <seealso cref="CLog.Framework.Services.Models.ResponseBase" />
    [DataContract]
    public sealed class LogoutResponse : ResponseBase
    {
    }
}
