using System;
using System.Collections.Generic;
using System.Linq;
using UIKit;
using Foundation;
using CoreGraphics;
using EnterpriseApps.Portable.ViewModel;
using Microsoft.Practices.ServiceLocation;
using EnterpriseApps.Portable.Model;
using System.Threading.Tasks;


namespace EnterpriseApps.iOS
{
	public partial class MasterViewController : UITableViewController, IUISearchResultsUpdating
	{
		public DetailViewController DetailViewController { get; set; }

		DataSource dataSource;
		private UsersViewModel _usersViewModel = ServiceLocator.Current.GetInstance<UsersViewModel>();
		private UISearchController _searchController;
		private List<User> _filteredUsers;

		private UIColor _defaultNavigationBarColor;
		private UIColor _defaultNavigationBarTintColor;
		private UIImage _defaultNavigationBarShadowImage;

		public void UpdateSearchResultsForSearchController (UISearchController searchController)
		{
			var searchText = _searchController.SearchBar.Text;
			_filteredUsers = _usersViewModel.Users.ToList ().FindAll (e => e.FirstName.Contains (searchText) || e.LastName.Contains (searchText));

			TableView.Source = new DataSource (this, _filteredUsers);
			TableView.ReloadData ();
		}

		public MasterViewController (IntPtr handle) : base (handle)
		{
			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) {
				PreferredContentSize = new CGSize (320f, 600f);
				ClearsSelectionOnViewWillAppear = false;
			}
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			PreserveNavigationbarAppearence ();

			SplitViewController.PreferredDisplayMode = UISplitViewControllerDisplayMode.AllVisible;
			ConfigureSearchController ();

			DetailViewController = (DetailViewController)((UINavigationController)SplitViewController.ViewControllers [1]).TopViewController;

			if (_usersViewModel.Users != null) {
				_filteredUsers = _usersViewModel.Users.ToList ();
				TableView.Source = dataSource = new DataSource (this, _filteredUsers);
			}

			_usersViewModel.PropertyChanged += (sender, e) => {
				if(e.PropertyName == "Users" && ((UsersViewModel)sender).Users != null){
					_filteredUsers = _usersViewModel.Users.ToList ();
					TableView.Source = dataSource = new DataSource (this, _filteredUsers);
					TableView.ReloadData();
				}
			};
		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
			ChangeNavigationbarAppearenceForDetailView ();
		}

		public override void ViewWillAppear(bool animated){
			base.ViewWillAppear (animated);
			RestoreNavigationbarAppearence ();
		}

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
		}

		public override void PrepareForSegue (UIStoryboardSegue segue, NSObject sender)
		{
			if (segue.Identifier == "showDetail") {
				var indexPath = TableView.IndexPathForSelectedRow;
				var item = dataSource.Objects [indexPath.Row];
				var controller = (DetailViewController)((UINavigationController)segue.DestinationViewController).TopViewController;
				controller.SetDetailItem (item);
				controller.NavigationItem.LeftBarButtonItem = SplitViewController.DisplayModeButtonItem;
				controller.NavigationItem.LeftItemsSupplementBackButton = true;
			}
		}
			
		private void ConfigureSearchController(){
			_searchController = new UISearchController(searchResultsController:null);
			_searchController.SearchResultsUpdater = this;
			_searchController.DimsBackgroundDuringPresentation = false;
			_searchController.DefinesPresentationContext = false;
			_searchController.HidesNavigationBarDuringPresentation = false;
			_searchController.SearchBar.Placeholder = "[Suchen]";
			NavigationItem.TitleView = _searchController.SearchBar;
		}

		private void PreserveNavigationbarAppearence(){
			_defaultNavigationBarColor = NavigationController.NavigationBar.BarTintColor;
			_defaultNavigationBarTintColor = NavigationController.NavigationBar.TintColor;
			_defaultNavigationBarShadowImage = NavigationController.NavigationBar.ShadowImage;
		}

		private void ChangeNavigationbarAppearenceForDetailView(){
			NavigationController.NavigationBar.SetBackgroundImage (new UIImage (), UIBarMetrics.Default);
			NavigationController.NavigationBar.BarTintColor = BootStrapper.AccentColor;
			NavigationController.NavigationBar.TintColor = UIColor.White;
			NavigationController.NavigationBar.ShadowImage = new UIImage ();
		}

		private void RestoreNavigationbarAppearence(){
			NavigationController.NavigationBar.SetBackgroundImage (null, UIBarMetrics.Default);
			NavigationController.NavigationBar.BarTintColor = _defaultNavigationBarColor;
			NavigationController.NavigationBar.TintColor = _defaultNavigationBarTintColor;
			NavigationController.NavigationBar.ShadowImage = _defaultNavigationBarShadowImage;
		}
	}
}


