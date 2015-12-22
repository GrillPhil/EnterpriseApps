using EnterpriseApps.Portable.ViewModel;
using Foundation;
using Microsoft.Practices.ServiceLocation;
using UIKit;

namespace EnterpriseApps.iOS
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
	[Register ("AppDelegate")]
	public class AppDelegate : UIApplicationDelegate, IUISplitViewControllerDelegate
	{
		// class-level declarations

		public override UIWindow Window {
			get;
			set;
		}

		public override bool FinishedLaunching (UIApplication application, NSDictionary launchOptions)
		{
			BootStrapper.Init();

            var usersViewModel = ServiceLocator.Current.GetInstance<UsersViewModel>();
            usersViewModel.InitCommand.Execute(null);

			// Override point for customization after application launch.
			var splitViewController = (UISplitViewController)Window.RootViewController;
			splitViewController.WeakDelegate = this;
			return true;
		}
			
		[Export ("splitViewController:collapseSecondaryViewController:ontoPrimaryViewController:")]
		public bool CollapseSecondViewController (UISplitViewController splitViewController, UIViewController secondaryViewController, UIViewController primaryViewController)
		{
			if (secondaryViewController.GetType () == typeof(UINavigationController) &&
			    ((UINavigationController)secondaryViewController).TopViewController.GetType () == typeof(DetailViewController) &&
			    ((DetailViewController)((UINavigationController)secondaryViewController).TopViewController).DetailItem == null) {
				// Return YES to indicate that we have handled the collapse by doing nothing; the secondary controller will be discarded.
				return true;
			}
			return false;
		}
	}
}


