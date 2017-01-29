namespace CLog.Models
{
    /// <summary>
    /// Represents the container of the model constant values.
    /// </summary>
    public static class ModelConstants
    {
        public const string REGEX_EMAIL = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";

        public const string NAME_REGEX = "^[a-zA-Z]+$";

        public const string REGEX_PASSWORD = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,15}$";
    }
}
