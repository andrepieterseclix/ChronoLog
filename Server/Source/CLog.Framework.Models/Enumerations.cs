namespace CLog.Framework.Models
{
    /// <summary>
    /// Represents the state of an entity.
    /// </summary>
    public enum DataState : byte
    {
        None = 0,
        AwaitingApproval = 1,
        Approved = 2,
        Rejected = 3,
        Active = 4,
        Inactive = 5,
        Suspended = 6
    }
}
