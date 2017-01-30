using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Integration;

namespace CLog.UI.Framework.Testing.Controls
{
    public sealed class BindablePropertyGrid : WindowsFormsHost
    {
        #region Fields

        private readonly PropertyGrid propertyGrid;

        #endregion

        #region Constructor

        public BindablePropertyGrid()
        {
            propertyGrid = new PropertyGrid()
            {
                ToolbarVisible = false
            };

            Child = propertyGrid;
        }

        #endregion

        #region Properties

        public static DependencyProperty SelectedObjectProperty = DependencyProperty.Register("SelectedObject", typeof(object), typeof(BindablePropertyGrid), new PropertyMetadata(SelectedObject_Changed));

        public object SelectedObject
        {
            get { return GetValue(SelectedObjectProperty); }
            set { SetValue(SelectedObjectProperty, value); }
        }

        #endregion

        #region Methods

        private static void SelectedObject_Changed(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            BindablePropertyGrid control = obj as BindablePropertyGrid;
            if (control != null)
                control.propertyGrid.SelectedObject = control.SelectedObject;
        }

        #endregion
    }
}
