using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWNDParentTest.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private string source = string.Empty;
        public string Source
        {
            get => source;
            set
            {
                source = value;
                OnPropertyChanged();
            }
        }

        private string hwndArgsFormat = "-parentHWND {0}";
        public string HWNDArgsFormat
        {
            get => hwndArgsFormat;
            set
            {
                hwndArgsFormat = value;
                OnPropertyChanged();
            }
        }

        private string extraArguments = string.Empty;
        public string ExtraArguments
        {
            get => extraArguments;
            set
            {
                extraArguments = value;
                OnPropertyChanged();
            }
        }
    }
}
