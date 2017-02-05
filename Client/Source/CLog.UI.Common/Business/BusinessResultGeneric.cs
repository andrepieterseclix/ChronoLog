namespace CLog.UI.Common.Business
{
    /// <summary>
    /// Represent the business result that contains a result object.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="CLog.UI.Common.Business.BusinessResult" />
    public sealed class BusinessResult<T> : BusinessResult
        where T : class
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessResult{T}"/> class.
        /// </summary>
        public BusinessResult()
            : this(default(T))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessResult{T}"/> class.
        /// </summary>
        /// <param name="result">The result.</param>
        public BusinessResult(T result = default(T))
        {
            Result = result;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        /// <value>
        /// The result.
        /// </value>
        public T Result { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance has result.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has result; otherwise, <c>false</c>.
        /// </value>
        public bool HasResult
        {
            get { return Result != default(T); }
        }

        #endregion
    }
}
