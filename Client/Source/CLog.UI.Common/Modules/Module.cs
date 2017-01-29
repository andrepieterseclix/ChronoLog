using CLog.UI.Common.ViewModels;
using System;
using System.Windows.Controls;

namespace CLog.UI.Common.Modules
{
    /// <summary>
    /// Represents a UI module.
    /// </summary>
    public sealed class Module
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Module"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="userControl">The user control.</param>
        /// <param name="viewModel">The view model.</param>
        /// <exception cref="System.ArgumentNullException">
        /// </exception>
        public Module(string name, UserControl userControl, ViewModelBase viewModel)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            if (userControl == null)
                throw new ArgumentNullException(nameof(userControl));

            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

            Name = name;
            Control = userControl;
            ViewModel = viewModel;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets the control.
        /// </summary>
        /// <value>
        /// The control.
        /// </value>
        public UserControl Control { get; private set; }

        /// <summary>
        /// Gets the view model.
        /// </summary>
        /// <value>
        /// The view model.
        /// </value>
        public ViewModelBase ViewModel { get; private set; }

        #endregion
    }
}
