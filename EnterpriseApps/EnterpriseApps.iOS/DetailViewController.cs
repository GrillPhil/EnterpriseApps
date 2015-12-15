using System;

using UIKit;
using EnterpriseApps.Portable.Model;
using Microsoft.Practices.ServiceLocation;
using CoreGraphics;
using System.Threading.Tasks;

namespace EnterpriseApps.iOS
{
	public partial class DetailViewController : UIViewController
	{
		public object DetailItem { get; set; }
		private User _user;
		private ImageService _imageService = ServiceLocator.Current.GetInstance<ImageService>();

		//	Subviews
		private UIImageView _userPictureImageView;
		private UILabel _userNameLabel;
		private UILabel _emailHeaderLabel;
		private UILabel _userEmailLabel;
		private UILabel _phoneHeaderLabel;
		private UILabel _userPhoneLabel;
		private UILabel _cellHeaderLabel;
		private UILabel _userCellLabel;


		public DetailViewController (IntPtr handle) : base (handle)
		{
		}

		public DetailViewController (object user) : base ()
		{
			DetailItem = user;
			_user = DetailItem as User;
		}

		public void SetDetailItem (object newDetailItem)
		{
			if (DetailItem != newDetailItem) {
				DetailItem = newDetailItem;
				_user = newDetailItem as User;
				// Update the view
				ConfigureView ();
			}
		}

		void ConfigureView ()
		{
			// Update the user interface for the detail item
			if (IsViewLoaded && DetailItem != null){
				Title = $"{_user.FirstName} {_user.LastName}";
				_userPictureImageView.Image = null;
				Task.Run(async() => {
					var image =  _imageService.GetUserPicture(_user);

					BeginInvokeOnMainThread(() =>{
						_userPictureImageView.Image = image;
					});
				});
				_userNameLabel.Text = $"{_user.FirstName} {_user.LastName}";
				_userNameLabel.SizeToFit ();

				_userEmailLabel.Text = _user.Email;
				_userEmailLabel.SizeToFit ();

				_phoneHeaderLabel.Text = "[Telefon]";
				_phoneHeaderLabel.SizeToFit ();

				_userPhoneLabel.Text = _user.Phone;
				_userPhoneLabel.SizeToFit ();

				_cellHeaderLabel.Text = "[Mobil]";
				_cellHeaderLabel.SizeToFit ();

				_userCellLabel.Text = _user.Cell;
				_userCellLabel.SizeToFit ();
			}
				//detailDescriptionLabel.Text = DetailItem.ToString ();
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			// Perform any additional setup after loading the view, typically from a nib.
			_userPictureImageView = new UIImageView(){
				BackgroundColor = UIColor.LightGray
			};

			_userNameLabel = new UILabel(){
				Font = UIFont.SystemFontOfSize(24),
				TextColor = UIColor.White,
				Lines = 0
			};
					
			_emailHeaderLabel = new UILabel(){
				Font = UIFont.SystemFontOfSize(18),
				TextColor = UIColor.FromWhiteAlpha((nfloat)1.0, (nfloat)0.7),
				Lines = 0,
				Text = "[Email]"
			};

			_userEmailLabel = new UILabel(){
				Font = UIFont.SystemFontOfSize(18),
				TextColor = UIColor.White,
				Lines = 0
			};

			_phoneHeaderLabel = new UILabel(){
				Font = UIFont.SystemFontOfSize(18),
				TextColor = UIColor.FromWhiteAlpha((nfloat)1.0, (nfloat)0.7),
				Lines = 0,
				Text = "[Telefon]"
			};


			_userPhoneLabel = new UILabel(){
				Font = UIFont.SystemFontOfSize(18),
				TextColor = UIColor.White,
				Lines = 0
			};


			_cellHeaderLabel = new UILabel(){
				Font = UIFont.SystemFontOfSize(18),
				TextColor = UIColor.FromWhiteAlpha((nfloat)1.0, (nfloat)0.7),
				Lines = 0,
				Text = "[Mobil]"
			};

			_userCellLabel = new UILabel(){
				Font = UIFont.SystemFontOfSize(18),
				TextColor = UIColor.White,
				Lines = 0
			};


			View.AddSubviews (
				_userPictureImageView, 
				_userNameLabel, 
				_emailHeaderLabel,
				_userEmailLabel, 
				_phoneHeaderLabel, 
				_userPhoneLabel,
				_cellHeaderLabel, 
				_userCellLabel);
			
			ConfigureView ();
			LayoutSubviews ();
		}

		void LayoutSubviews ()
		{
			_userPictureImageView.Frame = new CGRect(0, 0, 150,150);
			_userPictureImageView.Layer.CornerRadius = _userPictureImageView.Frame.Width / 2.0f;
			_userPictureImageView.Layer.MasksToBounds = true;

			//	User image constraints
			_userPictureImageView.TranslatesAutoresizingMaskIntoConstraints = false;
			SetLeadingConstraint (_userPictureImageView, View, (nfloat)30.0);
			var userPictureLeadingConstraint = NSLayoutConstraint.Create (_userPictureImageView, NSLayoutAttribute.LeftMargin, NSLayoutRelation.Equal, View, NSLayoutAttribute.Left, (nfloat)1.0, (nfloat)(40.0));
			userPictureLeadingConstraint.Active = true;
			var userPictureTopMarginConstraint = NSLayoutConstraint.Create (_userPictureImageView, NSLayoutAttribute.TopMargin, NSLayoutRelation.Equal, View, NSLayoutAttribute.Top, (nfloat)1.0, (nfloat)100.0);
			userPictureTopMarginConstraint.Active = true;
			SetHeightConstraint (_userPictureImageView, (nfloat)150.0);
			SetWidthConstraint (_userPictureImageView, (nfloat)150.0);

			//	User name label constraints
			_userNameLabel.TranslatesAutoresizingMaskIntoConstraints = false;
			var userNameLeadingConstraint = NSLayoutConstraint.Create (_userNameLabel, NSLayoutAttribute.LeftMargin, NSLayoutRelation.Equal, _userPictureImageView, NSLayoutAttribute.Right, (nfloat)1.0, (nfloat)(30.0));
			userNameLeadingConstraint.Active = true;
			var userNameCenterYConstraint = NSLayoutConstraint.Create (_userNameLabel, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, _userPictureImageView, NSLayoutAttribute.CenterY, (nfloat)1.0, (nfloat)0.0);
			userNameCenterYConstraint.Active = true;
			SetHeightConstraint (_userNameLabel, (nfloat)150.0);
			SetTrailingConstraint (_userNameLabel, View, (nfloat)(-40));

			//Email header constraints
			_emailHeaderLabel.TranslatesAutoresizingMaskIntoConstraints = false;
			SetLeadingConstraint (_emailHeaderLabel, View, (nfloat)40.0);
			SetTopMarginConstraint(_emailHeaderLabel, _userPictureImageView, (nfloat)30.0);
			SetHeightConstraint(_emailHeaderLabel, (nfloat)30.0);
			SetTrailingConstraint(_emailHeaderLabel, View,(nfloat)(-40.0));


			//	User email label constraints
			_userEmailLabel.TranslatesAutoresizingMaskIntoConstraints = false;
			SetLeadingConstraint (_userEmailLabel, View, (nfloat)40.0);
			SetTopMarginConstraint(_userEmailLabel, _emailHeaderLabel, (nfloat)5.0);
			SetHeightConstraint(_userEmailLabel, (nfloat)30.0);
			SetTrailingConstraint (_userEmailLabel, View, (nfloat)(-40.0));

			//Phone header constraints
			_phoneHeaderLabel.TranslatesAutoresizingMaskIntoConstraints = false;
			SetLeadingConstraint (_phoneHeaderLabel, View, (nfloat)40.0);
			SetTopMarginConstraint(_phoneHeaderLabel, _userEmailLabel, (nfloat)30.0);
			SetHeightConstraint(_phoneHeaderLabel, (nfloat)30.0);
			SetTrailingConstraint(_phoneHeaderLabel, View,(nfloat)(-40.0));


			//	User phone label constraints
			_userPhoneLabel.TranslatesAutoresizingMaskIntoConstraints = false;
			SetLeadingConstraint (_userPhoneLabel, View, (nfloat)40.0);
			SetTopMarginConstraint(_userPhoneLabel, _phoneHeaderLabel, (nfloat)5.0);
			SetHeightConstraint(_userPhoneLabel, (nfloat)30.0);
			SetTrailingConstraint (_userPhoneLabel, View, (nfloat)(-40.0));

			//Cell header constraints
			_cellHeaderLabel.TranslatesAutoresizingMaskIntoConstraints = false;
			SetLeadingConstraint (_cellHeaderLabel, View, (nfloat)40.0);
			SetTopMarginConstraint(_cellHeaderLabel, _userPhoneLabel, (nfloat)30.0);
			SetHeightConstraint(_cellHeaderLabel, (nfloat)30.0);
			SetTrailingConstraint(_cellHeaderLabel, View,(nfloat)(-40.0));


			//	User cell label constraints
			_userCellLabel.TranslatesAutoresizingMaskIntoConstraints = false;
			SetLeadingConstraint (_userCellLabel, View, (nfloat)40.0);
			SetTopMarginConstraint(_userCellLabel, _cellHeaderLabel, (nfloat)5.0);
			SetHeightConstraint(_userCellLabel, (nfloat)30.0);
			SetTrailingConstraint (_userCellLabel, View, (nfloat)(-40.0));

		}

		private void SetLeadingConstraint(UIView view, UIView relativeView, nfloat margin){
			var constraint = NSLayoutConstraint.Create (view, NSLayoutAttribute.LeftMargin, NSLayoutRelation.Equal, relativeView, NSLayoutAttribute.Left, (nfloat)1.0, margin);
			constraint.Active = true;
		}

		private void SetTopMarginConstraint(UIView view, UIView relativeView, nfloat margin){
			var constraint = NSLayoutConstraint.Create (view, NSLayoutAttribute.TopMargin, NSLayoutRelation.Equal, relativeView, NSLayoutAttribute.Bottom, (nfloat)1.0, margin);
			constraint.Active = true;
		}

		private void SetHeightConstraint(UIView view, nfloat height){
			var constraint = NSLayoutConstraint.Create (view, NSLayoutAttribute.Height, NSLayoutRelation.Equal, null, NSLayoutAttribute.NoAttribute, (nfloat)1.0, height);
			constraint.Active = true;
		}

		private void SetTrailingConstraint(UIView view, UIView relativeView,nfloat trailingSpace){
			var constraint = NSLayoutConstraint.Create (view, NSLayoutAttribute.Right, NSLayoutRelation.Equal, relativeView, NSLayoutAttribute.Right, (nfloat)1.0, trailingSpace);
			constraint.Active = true;
		}

		private void SetWidthConstraint(UIView view, nfloat width){
			var constraint = NSLayoutConstraint.Create (view, NSLayoutAttribute.Width, NSLayoutRelation.Equal, null, NSLayoutAttribute.NoAttribute, (nfloat)1.0, width);
			constraint.Active = true;
		}
			
		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}


