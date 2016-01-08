
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using EnterpriseApps.Portable.Model;
using EnterpriseApps.Portable.ViewModel;
using Microsoft.Practices.ServiceLocation;
using Android.Support.Design.Widget;
using Java.Net;
using Android.Graphics;
using System.Threading.Tasks;
using Java.Lang;

namespace EnterpriseApps.Droid
{
	/**
 * A fragment representing a single User detail screen.
 * This fragment is either contained in a {@link UserListActivity}
 * in two-pane mode (on tablets) or a {@link UserDetailActivity}
 * on handsets.
 */
	public class UserDetailFragment:Android.Support.V4.App.Fragment {

		private UserViewModel _userViewModel = ServiceLocator.Current.GetInstance<UserViewModel>();

		/**
     	* The content this fragment is presenting.
     	*/
		private User _user;

		/**
     	* Mandatory empty constructor for the fragment manager to instantiate the
     	* fragment (e.g. upon screen orientation changes).
     	*/
		public UserDetailFragment() {
		}

		public override void OnCreate(Bundle savedInstanceState) {
			base.OnCreate(savedInstanceState);
			_user = _userViewModel.User;
		}


		public override View OnCreateView(LayoutInflater inflater, ViewGroup container,
			Bundle savedInstanceState) {
			View rootView = inflater.Inflate(Resource.Layout.user_detail, container, false);

			if (_user != null) {
				((TextView)rootView.FindViewById (Resource.Id.user_name)).Text = $"{_user.FirstName} {_user.LastName}";
				((TextView)rootView.FindViewById (Resource.Id.mail_header)).Text = $"[Email]";
				((TextView)rootView.FindViewById (Resource.Id.user_mail)).Text = _user.Email;
				((TextView)rootView.FindViewById (Resource.Id.phone_header)).Text = $"[Telefon]";
				((TextView)rootView.FindViewById (Resource.Id.user_phone)).Text = _user.Phone;
				((TextView)rootView.FindViewById (Resource.Id.cell_header)).Text = $"[Mobil]";
				((TextView)rootView.FindViewById (Resource.Id.user_cell)).Text = _user.Cell;

				var imageView = (ImageView)rootView.FindViewById (Resource.Id.user_image);

				Handler handler = new Handler (Context.MainLooper);
				Task.Run (() => {
					var imageBitmap = ((ImageService)ServiceLocator.Current.GetInstance<ImageService>()).GetUserPicture(_user);
					Runnable runnable = new Runnable (() => {
						imageView.SetImageBitmap (imageBitmap);
					});
					handler.Post (runnable);
				});
			}
			return rootView;
		}
}
		
}

