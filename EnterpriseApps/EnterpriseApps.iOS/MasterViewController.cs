using System;
using System.Collections.Generic;
using System.Linq;
using UIKit;
using Foundation;
using CoreGraphics;
using EnterpriseApps.Portable.Model;
using EnterpriseApps.Portable.ViewModel;
using Microsoft.Practices.ServiceLocation;
using System.Threading.Tasks;

namespace EnterpriseApps.iOS
{
	public partial class MasterViewController : UITableViewController
	{
		public DetailViewController DetailViewController { get; set; }

		DataSource dataSource;
	    private UsersViewModel _usersViewModel = ServiceLocator.Current.GetInstance<UsersViewModel>();


		public MasterViewController (IntPtr handle) : base (handle)
		{
			Title = NSBundle.MainBundle.LocalizedString ("Master", "Master");
			
			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) {
				PreferredContentSize = new CGSize (320f, 600f);
				ClearsSelectionOnViewWillAppear = false;
			}
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			// Perform any additional setup after loading the view, typically from a nib.
			NavigationItem.LeftBarButtonItem = EditButtonItem;

			var addButton = new UIBarButtonItem (UIBarButtonSystemItem.Add, AddNewItem);
			addButton.AccessibilityLabel = "addButton";
			NavigationItem.RightBarButtonItem = addButton;

			DetailViewController = (DetailViewController)((UINavigationController)SplitViewController.ViewControllers [1]).TopViewController;

			if (_usersViewModel.Users != null) {
				TableView.Source = dataSource = new DataSource (this, _usersViewModel.Users.ToList ());
			}
			_usersViewModel.PropertyChanged += (sender, e) => {
				if(e.PropertyName == "Users" && ((UsersViewModel)sender).Users != null){
					TableView.Source = dataSource = new DataSource (this, _usersViewModel.Users.ToList ());
					TableView.ReloadData();
				}
			};

		}

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}

		void AddNewItem (object sender, EventArgs args)
		{
			//dataSource.Objects.Insert (0, DateTime.Now);

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

		class DataSource : UITableViewSource
		{
			static readonly NSString CellIdentifier = new NSString ("Cell");
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
					if(cell.UserImageView.Image == null){
						Task.Run(async() => {
							var indPath = indexPath;
							var image = await _imageService.GetUserThumbnailAsync(((User)_objects [indexPath.Row]));

							tableView.BeginInvokeOnMainThread(() =>{
								if(cell.UserNameLabel.Text == $"{currentUser.FirstName} {currentUser.LastName}")
									cell.UserImageView.Image = image;
								//cell.SetNeedsDisplay();
							});


						});
					}
				}catch(Exception ex){
					Console.WriteLine (ex.StackTrace);
				}
				return cell;
			}

			public override bool CanEditRow (UITableView tableView, NSIndexPath indexPath)
			{
				// Return false if you do not want the specified item to be editable.
				return false;
			}

			public override void CommitEditingStyle (UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
			{
				//if (editingStyle == UITableViewCellEditingStyle.Delete) {
				//	// Delete the row from the data source.
				//	objects.RemoveAt (indexPath.Row);
				//	controller.TableView.DeleteRows (new [] { indexPath }, UITableViewRowAnimation.Fade);
				//} else if (editingStyle == UITableViewCellEditingStyle.Insert) {
				//	// Create a new instance of the appropriate class, insert it into the array, and add a new row to the table view.
				//}
			}

			public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
			{
				if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad)
					_controller.DetailViewController.SetDetailItem (_objects [indexPath.Row]);
			}
		}
	}
}


