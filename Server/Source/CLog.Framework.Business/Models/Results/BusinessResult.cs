using System.Collections.Generic;

namespace CLog.Framework.Business.Models.Results
{
    /// <summary>
    /// Represents the business result.
    /// </summary>
    public class BusinessResult
    {
        #region Properties

        /// <summary>
        /// Gets the errors.
        /// </summary>
        /// <value>
        /// The errors.
        /// </value>
        public List<ErrorMessage> Errors { get; } = new List<ErrorMessage>();

        /// <summary>
        /// Gets a value indicating whether this instance has errors.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has errors; otherwise, <c>false</c>.
        /// </value>
        public bool HasErrors
        {
            get { return Errors.Count > 0; }
        }

        #endregion
    }
}
