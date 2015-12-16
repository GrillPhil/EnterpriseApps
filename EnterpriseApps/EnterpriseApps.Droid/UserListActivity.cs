
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
using Android.Support.V7.Widget;
using Android.Support.V7.App;
using EnterpriseApps.Portable.ViewModel;
using Microsoft.Practices.ServiceLocation;
using EnterpriseApps.Portable.Model;

namespace EnterpriseApps.Droid
{
	[Activity (Label = "UserListActivity", MainLauncher = true, Icon = "@drawable/icon")]			
	public class UserListActivity : AppCompatActivity
	{
		private bool mTwoPane;

		private UsersViewModel _usersViewModel;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			// Create your application here
			BootStrapper.Init();
			_usersViewModel = ServiceLocator.Current.GetInstance<UsersViewModel> ();


			SetContentView (Resource.Layout.activity_user_list);
			var toolbar = (Android.Support.V7.Widget.Toolbar)FindViewById (Resource.Id.toolbar);
			SetSupportActionBar(toolbar);

			var recyclerView = FindViewById (Resource.Id.user_list);
			_usersViewModel.PropertyChanged += (sender, e) => {
				if(e.PropertyName == "Users" && _usersViewModel.Users != null){
					SetupRecyclerView ((RecyclerView)recyclerView);
				}
			};
			_usersViewModel.InitCommand.Execute (null);

			if (FindViewById(Resource.Id.user_detail_container) != null) {
				// The detail container view will be present only in the
				// large-screen layouts (res/values-w900dp).
				// If this view is present, then the
				// activity should be in two-pane mode.
				mTwoPane = true;
			}

		}

		private void SetupRecyclerView(RecyclerView recyclerView) {
			recyclerView.SetAdapter(new SimpleItemRecyclerViewAdapter(_usersViewModel.Users.ToList()));
		}


	}

	public class SimpleItemRecyclerViewAdapter:RecyclerView.Adapter {

		private List<User> users;

		#region implemented abstract members of Adapter

		public override int ItemCount {
			get{ return users.Count();}
		}

		#endregion

		public SimpleItemRecyclerViewAdapter(List<User> items) {
			users = items;
		}


		public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType) {
			View view = LayoutInflater.From (parent.Context).Inflate (Resource.Layout.user_list_content, parent, false);
			return new ViewHolder(view);
		}


		public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position) {
			((ViewHolder)holder).mItem = users[position];
			((ViewHolder)holder).mIdView.Text =  position + "";
			((ViewHolder)holder).mContentView.Text =  $"{(users[position]).FirstName} {(users[position]).LastName}";


			//((ViewHolder)holder).mView.SetOnClickListener (new ClickListener (mTwoPane));
		}

//			internal class ClickListener:View.IOnClickListener{
//					#region IOnClickListener implementation
//
//			private bool mTwoPane;
//
//			public ClickListener(bool mTwoPane){
//				mTwoPane = mTwoPane;
//			}
//
//					public void OnClick (View v)
//					{
//						if (mTwoPane) {
//							var arguments = new Bundle();
//							arguments.PutString(UserDetailFragment.ARG_ITEM_ID.ToString, ((ViewHolder)holder).mItem);
//							UserDetailFragment fragment = new UserDetailFragment();
//					fragment.Argumnets = arguments;
//							SupportFragmentManager.BeginTransaction()
//								.Replace(Resource.Id.user_detail_container, fragment)
//								.Commit();
//						} else {
//							Context context = vo;
//					Intent intent = new Intent(context, typeof(UserDetailActivity));
//							intent.putExtra(UserDetailFragment.ARG_ITEM_ID, holder.mItem.id);
//
//							context.startActivity(intent);
//						}
//
//					}
//
//					#endregion
//
//					#region IDisposable implementation
//
//					public void Dispose ()
//					{
//						throw new NotImplementedException ();
//					}
//
//					#endregion
//
//					#region IJavaObject implementation
//
//					public IntPtr Handle {
//						get {
//							throw new NotImplementedException ();
//						}
//					}
//
//					#endregion
//
//
//
//				}

		public class ViewHolder:RecyclerView.ViewHolder {
			public  View mView;
			public  TextView mIdView;
			public  TextView mContentView;
			public object mItem;

			public ViewHolder(View view):base(view) {
				
				mView = view;
				mIdView = (TextView) view.FindViewById(Resource.Id.id);
				mContentView = (TextView) view.FindViewById(Resource.Id.content);
			}


			public override String ToString() {
				return base.ToString() + " '" + mContentView.Text + "'";
			}
		}
	}

	

}

