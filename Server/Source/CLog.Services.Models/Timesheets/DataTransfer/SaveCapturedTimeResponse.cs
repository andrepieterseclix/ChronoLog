using CLog.Framework.Services.Models;
using System.Runtime.Serialization;

namespace CLog.Services.Models.Timesheets.DataTransfer
{
    /// <summary>
    /// Represents the save captured time response model.
    /// </summary>
    /// <seealso cref="CLog.Framework.Services.Models.ResponseBase" />
    [DataContract]
    public sealed class SaveCapturedTimeResponse : ResponseBase
    {
    }
}
