using System.Windows;

namespace CLog.UI.Framework.Testing.Views
{
    /// <summary>
    /// Interaction logic for ObjectPropertyGridWindow.xaml
    /// </summary>
    public partial class ObjectPropertyGridWindow : Window
    {
        public ObjectPropertyGridWindow(bool hideButtons)
        {
            InitializeComponent();

            ButtonPanel.Visibility = hideButtons ? Visibility.Collapsed : Visibility.Visible;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
