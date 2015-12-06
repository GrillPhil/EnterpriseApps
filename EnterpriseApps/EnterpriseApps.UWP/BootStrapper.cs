using EnterpriseApps.Portable.Service;
using EnterpriseApps.Portable.ViewModel;
using EnterpriseApps.UWP.Service;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseApps.UWP
{
    public class BootStrapper
    {
        public BootStrapper()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            // Register Services
            SimpleIoc.Default.Register<IHttpService>(() => 
            {
                return new HttpService("http://api.randomuser.me/");
            });
            SimpleIoc.Default.Register<IMappingService, MappingService>();
            SimpleIoc.Default.Register<IResourceService, ResourceService>();
            SimpleIoc.Default.Register<IDialogService, DialogService>();
            SimpleIoc.Default.Register<IUserRepository, UserRepository>();

            // Register ViewModels
            SimpleIoc.Default.Register<UsersViewModel>();
            SimpleIoc.Default.Register<UserViewModel>();
        }

        public UsersViewModel UsersViewModel
        {
            get
            {
                return SimpleIoc.Default.GetInstance<UsersViewModel>();
            }
        }

        public UserViewModel UserViewModel
        {
            get
            {
                return SimpleIoc.Default.GetInstance<UserViewModel>();
            }
        }
    }
}
