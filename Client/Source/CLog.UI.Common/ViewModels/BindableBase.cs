using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CLog.UI.Common.ViewModels
{
    public abstract class BindableBase : INotifyPropertyChanged
    {
        #region Methods

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

        #endregion

        #region Events

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
