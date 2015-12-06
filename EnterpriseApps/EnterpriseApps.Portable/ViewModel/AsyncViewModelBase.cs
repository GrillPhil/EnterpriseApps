using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseApps.Portable.ViewModel
{
    public abstract class AsyncViewModelBase : ViewModelBase
    {
        private bool _isLoading;
        private string _loadingMessage;

        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }

            set
            {
                _isLoading = value;
                RaisePropertyChanged();
            }
        }

        public string LoadingMessage
        {
            get
            {
                return _loadingMessage;
            }

            set
            {
                _loadingMessage = value;
                RaisePropertyChanged();
            }
        }
    }
}
