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
using System.Threading.Tasks;
using Java.Lang;
using Android.Graphics;

namespace EnterpriseApps.Droid
{
	[Activity (Label = "Userliste", MainLauncher = true, Icon = "@drawable/icon")]			
	public class UserListActivity : AppCompatActivity, Android.Support.V7.Widget.SearchView.IOnQueryTextListener
	{
		private static bool mTwoPane;

		private UsersViewModel _usersViewModel;
		private List<User> _filteredUsers;
		private RecyclerView _recyclerView;
		private SimpleItemRecyclerViewAdapter _adapter;


		/// <summary>
		/// Raises the query text change event.
		/// Filters the user list in the UsersViewModel by wether the entered string is contained in the users first/last name 
		/// </summary>
		/// <param name="newText">Text entered in the SearchView by the user</param>
		public  bool OnQueryTextChange(string newText){
			_recyclerView = FindViewById<RecyclerView> (Resource.Id.user_list);
			_filteredUsers = _usersViewModel.Users.ToList ().FindAll (e => (e.FirstName.ToLower() + " " + e.LastName.ToLower()).Contains (newText.ToLower()));
			SetupRecyclerView (_recyclerView);
			return true;
		}

		/// <summary>
		/// Raises the query text submit event.
		/// Never called in this project, because SearchView.SubmitButtonEnabled is set to false.
		/// </summary>
		/// <param name="query">Query.</param>
		public  bool OnQueryTextSubmit(string query) {
			return false;
		}

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			BootStrapper.Init ();
			_usersViewModel = ServiceLocator.Current.GetInstance<UsersViewModel> ();

			// Set the content for this activity.
			SetContentView (Resource.Layout.activity_user_list);

			var toolbar = (Android.Support.V7.Widget.Toolbar)FindViewById (Resource.Id.toolbar);
			SetSupportActionBar (toolbar);

			// Set the RecyclerView. If the device screen width is wider than 700dp, the user_list from Resources/layout-sw700dp is used.
			_recyclerView = FindViewById<RecyclerView> (Resource.Id.user_list);

			_usersViewModel.PropertyChanged += (sender, e) => {
				if (e.PropertyName == "Users" && _usersViewModel.Users != null) {
					SetupRecyclerView (_recyclerView);
				}
			};
			_usersViewModel.InitCommand.Execute (null);

			// Because the user_detail_container is only contained in the user_list if the device screen width is higher than 700dp,
			// it can be used as an indicator for the app being shown in split screen mode.
			if (FindViewById (Resource.Id.user_detail_container) != null) {
				mTwoPane = true;
			}

		}

		public override bool OnCreateOptionsMenu(IMenu menu){
			MenuInflater.Inflate (Resource.Menu.main_menu, menu);
			var searchView = (Android.Support.V7.Widget.SearchView) menu.FindItem (Resource.Id.action_search).ActionView;
			SearchManager searchManager = (SearchManager)GetSystemService (Context.SearchService);
			searchView.SetSearchableInfo(searchManager.GetSearchableInfo(this.ComponentName));
			searchView.SetOnQueryTextListener (this);
			return base.OnCreateOptionsMenu (menu);
		}

		protected override void OnPause ()
		{
			base.OnPause ();
		}

		private void SetupRecyclerView (RecyclerView recyclerView)
		{
			var supportFragmentManager = SupportFragmentManager;

			if (_filteredUsers == null)
				_filteredUsers = _usersViewModel.Users.ToList ();
			
			_adapter = new SimpleItemRecyclerViewAdapter (_filteredUsers, this.BaseContext, supportFragmentManager);
			recyclerView.SetAdapter (_adapter);
		}

		// A custom RecyclerViewAdapter that contains a list of the users to be shown.
		public class SimpleItemRecyclerViewAdapter:RecyclerView.Adapter
		{
			private List<User> _users;
			private Context _context;
			private Android.Support.V4.App.FragmentManager _fragmentManager;

			public override int ItemCount {
				get{ return _users.Count (); }
			}

			public SimpleItemRecyclerViewAdapter (List<User> items, Context context, Android.Support.V4.App.FragmentManager fragmentManager)
			{
				_users = items;
				_context = context;
				_fragmentManager = fragmentManager;	
			}


			public override RecyclerView.ViewHolder OnCreateViewHolder (ViewGroup parent, int viewType)
			{
				View view = LayoutInflater.From (parent.Context).Inflate (Resource.Layout.user_list_content, parent, false);
				return new ViewHolder (view);
			}


			public override void OnBindViewHolder (RecyclerView.ViewHolder holder, int position)
			{
				((ViewHolder)holder).mItem = _users [position];
				((ViewHolder)holder).mIdView.SetImageBitmap (null);

				//	set user image async
				Handler handler = new Handler (_context.MainLooper);
				Task.Run (() => {
					var imageBitmap = ((ImageService)ServiceLocator.Current.GetInstance<ImageService> ()).GetUserThumbnail (((ViewHolder)holder).mItem);
					Runnable runnable = new Runnable (() => {
						((ViewHolder)holder).mIdView.SetImageBitmap (imageBitmap);
					});
					handler.Post (runnable);
				});

				((ViewHolder)holder).mContentView.Text = $"{(_users[position]).FirstName} {(_users[position]).LastName}";
				((ViewHolder)holder).mView.SetOnClickListener (new ClickListener ((ViewHolder)holder, _fragmentManager));
			}

			/// <summary>
			/// EventListener for ClickEvents on an ViewHolder object.
			/// </summary>
			class ClickListener:Java.Lang.Object, View.IOnClickListener
			{
					
				private ViewHolder _holder;
				private Android.Support.V4.App.FragmentManager _fragmentManager;

				public ClickListener (ViewHolder holder, Android.Support.V4.App.FragmentManager fragmentManager)
				{
					_holder = holder;
					_fragmentManager = fragmentManager;
				}

				public void OnClick (View v)
				{
					if (mTwoPane) {
						ServiceLocator.Current.GetInstance<UsersViewModel> ().SelectUserCommand.Execute (_holder.mItem);
						UserDetailFragment fragment = new UserDetailFragment ();
						_fragmentManager.BeginTransaction().Replace (Resource.Id.user_detail_container, fragment).Commit ();
					} else {
						Context context = v.Context;
						ServiceLocator.Current.GetInstance<UsersViewModel> ().SelectUserCommand.Execute (_holder.mItem);
						Intent intent = new Intent (context, typeof(UserDetailActivity));
						context.StartActivity (intent);
					}

				}
			}

			// The ViewHolder is a kind like ListItem that presents a user object in the RecyclerView 
			public class ViewHolder:RecyclerView.ViewHolder
			{
				public  View mView;
				public  ImageView mIdView;
				public  TextView mContentView;
				public User mItem;

				public ViewHolder (View view) : base (view)
				{
					mView = view;
					mIdView = (ImageView)view.FindViewById (Resource.Id.user_thumbnail);
					mContentView = (TextView)view.FindViewById (Resource.Id.content);
				}


				public override string ToString ()
				{
					return base.ToString () + " '" + mContentView.Text + "'";
				}
			}
		}

	

	}
}
