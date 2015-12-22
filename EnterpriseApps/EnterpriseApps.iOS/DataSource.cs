using System;
using UIKit;
using System.Collections.Generic;
using EnterpriseApps.Portable.Model;
using Microsoft.Practices.ServiceLocation;
using Foundation;
using System.Threading.Tasks;
using EnterpriseApps.Portable.ViewModel;

namespace EnterpriseApps.iOS
{
	public class DataSource : UITableViewSource
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

				Task.Run(() => {
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

		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			var a = ServiceLocator.Current.GetInstance<UsersViewModel> ();
			a.SelectUserCommand.Execute (_objects [indexPath.Row]);

			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) {

				return;
			}
			_controller.SplitViewController.ShowDetailViewController(_controller.DetailViewController, _controller);
		}
	}
}

