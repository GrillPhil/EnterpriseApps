
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

namespace EnterpriseApps.Droid
{
	/**
 * A fragment representing a single User detail screen.
 * This fragment is either contained in a {@link UserListActivity}
 * in two-pane mode (on tablets) or a {@link UserDetailActivity}
 * on handsets.
 */
	public class UserDetailFragment:Android.Support.V4.App.Fragment {
		/**
     * The fragment argument representing the item ID that this fragment
     * represents.
     */
		public static readonly int ARG_ITEM_ID = 0;
		private UsersViewModel _usersViewModel = ServiceLocator.Current.GetInstance<UsersViewModel>();
		/**
     * The dummy content this fragment is presenting.
     */
		private User mItem;

		/**
     * Mandatory empty constructor for the fragment manager to instantiate the
     * fragment (e.g. upon screen orientation changes).
     */
		public UserDetailFragment() {
		}

		public override void OnCreate(Bundle savedInstanceState) {
			base.OnCreate(savedInstanceState);
			if (Arguments.ContainsKey(ARG_ITEM_ID.ToString())) {
				// Load the dummy content specified by the fragment
				// arguments. In a real-world scenario, use a Loader
				// to load content from a content provider.
				mItem = _usersViewModel.Users[ARG_ITEM_ID];

				Activity activity = this.Activity;
				var appBarLayout = (CollapsingToolbarLayout) activity.FindViewById(Resource.Id.toolbar_layout);
				if (appBarLayout != null) {
					appBarLayout.SetTitle(mItem.LastName);
				}
			}
		}


		public override View OnCreateView(LayoutInflater inflater, ViewGroup container,
			Bundle savedInstanceState) {
			View rootView = inflater.Inflate(Resource.Layout.user_detail, container, false);

			// Show the dummy content as text in a TextView.
			if (mItem != null) {
				((TextView)rootView.FindViewById (Resource.Id.user_detail)).Text = mItem.FirstName;
			}

			return rootView;
		}
}
}

