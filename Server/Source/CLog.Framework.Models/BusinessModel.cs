using System;

namespace CLog.Framework.Models
{
    /// <summary>
    /// Represents the base business model which every business model should be derived from.
    /// </summary>
    public abstract class BusinessModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessModel"/> class.
        /// </summary>
        public BusinessModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessModel"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">The id must be greater than or equal to 0.</exception>
        protected BusinessModel(long id)
        {
            if (id < 0)
                throw new ArgumentOutOfRangeException(nameof(id));

            Id = id;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public long Id { get; set; }

        #endregion
    }
}
