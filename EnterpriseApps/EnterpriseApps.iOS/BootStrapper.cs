using System;
using EnterpriseApps.iOS.Services;
using EnterpriseApps.Portable.Service;
using EnterpriseApps.Portable.ViewModel;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using IResourceService = System.ComponentModel.Design.IResourceService;


namespace EnterpriseApps.iOS
{
	public static class BootStrapper
	{
		public static void Init ()
		{
			
			ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

			// Register Services
			SimpleIoc.Default.Register<IHttpService>(() => 
				{
					return new HttpService("http://api.randomuser.me/");
				});
			SimpleIoc.Default.Register<IMappingService, MappingService>();
			SimpleIoc.Default.Register<EnterpriseApps.Portable.Service.IResourceService, ResourceService>();
			SimpleIoc.Default.Register<IDialogService, DialogService>();
			SimpleIoc.Default.Register<IUserRepository, UserRepository>();

			// Register ViewModels
			SimpleIoc.Default.Register<UsersViewModel>();
		    SimpleIoc.Default.Register<UserViewModel>();

		}
	}
}

