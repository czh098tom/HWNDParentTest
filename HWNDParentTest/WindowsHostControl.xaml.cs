using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// WindowsHostControl.xaml 的交互逻辑
    /// </summary>
    public partial class WindowsHostControl : UserControl, IDisposable
    {
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
            nameof(Source),
            typeof(string),
            typeof(WindowsHostControl),
            new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty HWNDArgsFormatProperty = DependencyProperty.Register(
            nameof(HWNDArgsFormat),
            typeof(string),
            typeof(WindowsHostControl),
            new PropertyMetadata("-parentHWND {0}"));

        public static readonly DependencyProperty ExtraArgumentsProperty = DependencyProperty.Register(
            nameof(ExtraArguments),
            typeof(string),
            typeof(WindowsHostControl),
            new PropertyMetadata(string.Empty));

        public string Source
        {
            get => (string)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        public string HWNDArgsFormat
        {
            get => (string)GetValue(HWNDArgsFormatProperty);
            set => SetValue(HWNDArgsFormatProperty, value);
        }

        public string ExtraArguments
        {
            get => (string)GetValue(ExtraArgumentsProperty);
            set => SetValue(ExtraArgumentsProperty, value);
        }

        private WindowsHost? currentHost;

        public WindowsHostControl()
        {
            InitializeComponent();
        }

        public void Refresh()
        {
            InitializeWindowsHostForChild(Source);
        }

        private void InitializeWindowsHostForChild(string name)
        {
            currentHost?.Dispose();
            if (currentHost != null)
            {
                RemoveLogicalChild(currentHost);
                RemoveVisualChild(currentHost);
            }
            if (System.IO.Path.Exists(name))
            {
                currentHost = new WindowsHost(name, HWNDArgsFormat, ExtraArguments);
                AddChild(currentHost);
            }
            else
            {
                currentHost = null;
            }
        }

        public void Dispose()
        {
            currentHost?.Dispose();
        }
    }
}
