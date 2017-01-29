using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CLog.UI.Models
{
    public abstract class ModelBase : INotifyPropertyChanged, IDataErrorInfo
    {
        #region Fields

        private readonly Dictionary<string, string> _validationTable = new Dictionary<string, string>();

        #endregion

        #region Properties

        /// <summary>
        /// Gets an error message indicating what is wrong with this object.
        /// </summary>
        public string Error
        {
            get
            {
                if (_validationTable.Count < 1)
                    return null;

                return string.Join("\r\n", _validationTable.Values.Select(e => e));
            }
        }

        /// <summary>
        /// Gets the error message from validating the specified column, or <code>null</code> if it is valid.
        /// </summary>
        /// <value>
        /// The validation error message.
        /// </value>
        /// <param name="columnName">Name of the column.</param>
        /// <returns>The validation error message.</returns>
        public string this[string columnName]
        {
            get
            {
                string result = ValidateProperty(columnName);

                if (result == null)
                    _validationTable.Remove(columnName);
                else
                    _validationTable[columnName] = result;

                RaisePropertyChanged(nameof(Error));

                return result;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Validates the specified property when overridden in an inherited class.
        /// </summary>
        /// <remarks>
        /// The property is deemed valid if this method returns null.
        /// </remarks>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>The error string, or <code>null</code> if the property has a valid value.</returns>
        protected virtual string ValidateProperty(string propertyName)
        {
            return null;
        }

        /// <summary>
        /// Sets the property and invokes a change notification event through the <see cref="INotifyPropertyChanged"/> interface.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName]string propertyName = null)
        {
            if (Equals(field, value))
                return false;

            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            return true;
        }

        protected void RaisePropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
