
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Android.Support.V7.App;
using Android.Support.V4.App;
using Microsoft.Practices.ServiceLocation;
using EnterpriseApps.Portable.ViewModel;

namespace EnterpriseApps.Droid
{
	[Activity (Label = "Detail")]			
	public class UserDetailActivity : AppCompatActivity
	{
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			SetContentView (Resource.Layout.activity_user_detail);
			var toolbar = (Android.Support.V7.Widget.Toolbar)FindViewById (Resource.Id.detail_toolbar);
			SetSupportActionBar (toolbar);

			Android.Support.V7.App.ActionBar actionBar = SupportActionBar;
			// savedInstanceState is non-null when there is fragment state
			// saved from previous configurations of this activity
			// (e.g. when rotating the screen from portrait to landscape).
			// In this case, the fragment will automatically be re-added
			// to its container so we don't need to manually add it.
			// For more information, see the Fragments API guide at:
			//
			// http://developer.android.com/guide/components/fragments.html
			//
			if (savedInstanceState == null) {
				var fragment = new UserDetailFragment ();
				var fragmentTransaction = SupportFragmentManager.BeginTransaction ();
				fragmentTransaction.Add (Resource.Id.user_detail_container, fragment).Commit ();
				var a  = ServiceLocator.Current.GetInstance<UsersViewModel> ();
				var b = a.SelectedUser;
				SupportActionBar.Title = "[Detail]";
			}
		}


		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			int id = item.ItemId;
			if (id == Android.Resource.Id.Home) {
				// This ID represents the Home or Up button. In the case of this
				// activity, the Up button is shown. Use NavUtils to allow users
				// to navigate up one level in the application structure. For
				// more details, see the Navigation pattern on Android Design:
				//
				// http://developer.android.com/design/patterns/navigation.html#up-vs-back
				//
				NavUtils.NavigateUpTo (this, new Intent (this, typeof(UserListActivity)));
				return true;
			}
			return base.OnOptionsItemSelected (item);
		}
	}
}

