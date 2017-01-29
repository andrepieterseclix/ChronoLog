using System;

namespace CLog.UI.Common.Modules
{
    /// <summary>
    /// Represents the attribute that specified the order in which the modules will be loaded.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    public sealed class ModuleInitialiserOrderAttribute : Attribute
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleInitialiserOrderAttribute"/> class.
        /// </summary>
        /// <param name="order">The order.</param>
        public ModuleInitialiserOrderAttribute(int order)
        {
            Order = order;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the order.
        /// </summary>
        /// <value>
        /// The order.
        /// </value>
        public int Order { get; private set; }

        #endregion
    }
}
