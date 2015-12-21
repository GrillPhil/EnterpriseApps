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
	public class UserListActivity : AppCompatActivity
	{
		private static bool mTwoPane;

		private UsersViewModel _usersViewModel;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			BootStrapper.Init ();
			_usersViewModel = ServiceLocator.Current.GetInstance<UsersViewModel> ();
			SetContentView (Resource.Layout.activity_user_list);
			var toolbar = (Android.Support.V7.Widget.Toolbar)FindViewById (Resource.Id.toolbar);
			SetSupportActionBar (toolbar);
			toolbar.Title = "[Userliste]";

			var recyclerView = FindViewById (Resource.Id.user_list);
			_usersViewModel.PropertyChanged += (sender, e) => {
				if (e.PropertyName == "Users" && _usersViewModel.Users != null) {
					SetupRecyclerView ((RecyclerView)recyclerView);
				}
			};
			_usersViewModel.InitCommand.Execute (null);

			if (FindViewById (Resource.Id.user_detail_container) != null) {
				mTwoPane = true;
			}
		}

		protected override void OnPause ()
		{
			base.OnPause ();
		}

		private void SetupRecyclerView (RecyclerView recyclerView)
		{
			var a = SupportFragmentManager.BeginTransaction ();
			recyclerView.SetAdapter (new SimpleItemRecyclerViewAdapter (_usersViewModel.Users.ToList (), mTwoPane, this.BaseContext, a));
		}

		public class SimpleItemRecyclerViewAdapter:RecyclerView.Adapter
		{

			private List<User> users;
			private Context _context;
			private Android.Support.V4.App.FragmentTransaction _fragmentTransaction;


			public override int ItemCount {
				get{ return users.Count (); }
			}

			public SimpleItemRecyclerViewAdapter (List<User> items, bool mTwoPane, Context context, Android.Support.V4.App.FragmentTransaction fragmentTransaction)
			{
				users = items;
				_context = context;
				_fragmentTransaction = fragmentTransaction;		
			}


			public override RecyclerView.ViewHolder OnCreateViewHolder (ViewGroup parent, int viewType)
			{
				View view = LayoutInflater.From (parent.Context).Inflate (Resource.Layout.user_list_content, parent, false);
				return new ViewHolder (view);
			}


			public override void OnBindViewHolder (RecyclerView.ViewHolder holder, int position)
			{
				((ViewHolder)holder).mItem = users [position];
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

				((ViewHolder)holder).mContentView.Text = $"{(users[position]).FirstName} {(users[position]).LastName}";
				((ViewHolder)holder).mView.SetOnClickListener (new ClickListener ((ViewHolder)holder, _fragmentTransaction));
			}


			class ClickListener:Java.Lang.Object, View.IOnClickListener
			{
					
				private ViewHolder _holder;
				private Android.Support.V4.App.FragmentTransaction _fragmentTransaction;

				public ClickListener (ViewHolder holder, Android.Support.V4.App.FragmentTransaction fragmentTransaction)
				{
					_holder = holder;
					_fragmentTransaction = fragmentTransaction;
				}

				public void OnClick (View v)
				{
					if (mTwoPane) {
						var arguments = new Bundle ();
						arguments.PutString (UserDetailFragment.ARG_ITEM_ID.ToString (), _holder.mItem.FirstName);
						UserDetailFragment fragment = new UserDetailFragment ();
						fragment.Arguments = arguments;
						_fragmentTransaction.Replace (Resource.Id.user_detail_container, fragment).Commit ();
					} else {
						Context context = v.Context;
						ServiceLocator.Current.GetInstance<UsersViewModel> ().SelectUserCommand.Execute (_holder.mItem);
						Intent intent = new Intent (context, typeof(UserDetailActivity));
						intent.PutExtra (UserDetailFragment.ARG_ITEM_ID, _holder.mItem.FirstName);
						context.StartActivity (intent);
					}
				}
			}

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
