using HWNDParentTest.Properties;
using HWNDParentTest.ViewModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HWNDParentTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly MainViewModel? vm;
        public MainWindow()
        {
            InitializeComponent();
            if (DataContext is MainViewModel vm)
            {
                this.vm = vm;
                vm.Source = Settings.Default.Source;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            windowsHost.Refresh();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Settings.Default["Source"] = vm?.Source ?? string.Empty;
            Settings.Default.Save();
            windowsHost.Dispose();
        }
    }
}