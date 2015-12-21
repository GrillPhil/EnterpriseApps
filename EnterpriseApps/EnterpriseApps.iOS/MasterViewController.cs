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
			_defaultNavigationBarColor = NavigationController.NavigationBar.BarTintColor;
			_defaultNavigationBarTintColor = NavigationController.NavigationBar.TintColor;
			_defaultNavigationBarShadowImage = NavigationController.NavigationBar.ShadowImage;

			SplitViewController.PreferredDisplayMode = UISplitViewControllerDisplayMode.AllVisible;
			_searchController = new UISearchController(searchResultsController:null);
			_searchController.SearchResultsUpdater = this;
			_searchController.DimsBackgroundDuringPresentation = false;
			_searchController.DefinesPresentationContext = false;
			_searchController.HidesNavigationBarDuringPresentation = false;
			_searchController.SearchBar.Placeholder = "[Suchen]";
			NavigationItem.TitleView = _searchController.SearchBar;


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

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
		}

		void AddNewItem (object sender, EventArgs args)
		{
			using (var indexPath = NSIndexPath.FromRowSection (0, 0))
				TableView.InsertRows (new [] { indexPath }, UITableViewRowAnimation.Automatic);
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

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
			NavigationController.NavigationBar.SetBackgroundImage (new UIImage (), UIBarMetrics.Default);
			NavigationController.NavigationBar.BarTintColor = BootStrapper.AccentColor;
			NavigationController.NavigationBar.TintColor = UIColor.White;
			NavigationController.NavigationBar.ShadowImage = new UIImage ();
		}

		public override void ViewWillAppear(bool animated){
			base.ViewWillAppear (animated);
			NavigationController.NavigationBar.SetBackgroundImage (null, UIBarMetrics.Default);
			NavigationController.NavigationBar.BarTintColor = _defaultNavigationBarColor;
			NavigationController.NavigationBar.TintColor = _defaultNavigationBarTintColor;
			NavigationController.NavigationBar.ShadowImage = _defaultNavigationBarShadowImage;
		}

		class DataSource : UITableViewSource
		{
			static readonly string CellIdentifier = "UserCell";
			readonly List<User> _objects = new List<User> ();
			readonly MasterViewController _controller;
			private ImageService _imageService = ServiceLocator.Current.GetInstance<ImageService>();

			public DataSource (MasterViewController controller, List<User> objects)
			{
				this._controller = controller;
				this._objects = objects;
			}

			public IList<User> Objects {
				get { return _objects; }
			}

			// Customize the number of sections in the table view.
			public override nint NumberOfSections (UITableView tableView)
			{
				return 1;
			}

			public override nint RowsInSection (UITableView tableview, nint section)
			{
				return _objects.Count;
			}

			// Customize the appearance of table view cells.
			public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
			{
				var cell = tableView.DequeueReusableCell (CellIdentifier) as UserCell;
				if (cell == null)
					cell = new UserCell ((NSString)CellIdentifier);
				var currentUser = (User)_objects [indexPath.Row];
				try{
					((UserCell)cell).UserNameLabel.Text = currentUser.FirstName + " " + currentUser.LastName;
					((UserCell)cell).UserImageView.Image = null;

					Task.Run(async() => {
							var user = currentUser;
							var image = _imageService.GetUserThumbnailAsync(((User)_objects [indexPath.Row]));

							tableView.BeginInvokeOnMainThread(() =>{
								if(cell.UserNameLabel.Text == $"{user.FirstName} {user.LastName}")
									cell.UserImageView.Image = image;
							});


						});

				}catch(Exception ex){
					Console.WriteLine (ex.StackTrace);
				}
				return cell;
			}

			public override bool CanEditRow (UITableView tableView, NSIndexPath indexPath)
			{
				// Return false if you do not want the specified item to be editable.
				return true;
			}

			public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
			{
				var a = ServiceLocator.Current.GetInstance<UsersViewModel> ();
				a.SelectUserCommand.Execute (_objects [indexPath.Row]);

				if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) {

					return;
				}
//				_controller.NavigationController.PushViewController (_controller.DetailViewController.NavigationController, true);
				_controller.SplitViewController.ShowDetailViewController(_controller.DetailViewController, _controller);


			}
		}
	}
}


