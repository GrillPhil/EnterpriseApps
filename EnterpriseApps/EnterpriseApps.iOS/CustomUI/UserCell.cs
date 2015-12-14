using System;
using UIKit;
using Foundation;
using CoreGraphics;

namespace EnterpriseApps.iOS
{
	public class UserCell:UITableViewCell
	{
		private UILabel _userNameLabel;
		public UILabel UserNameLabel{
			get{ return _userNameLabel;}
			set{ _userNameLabel = value;}
		}
		private UIImageView _userImageView;
		public UIImageView UserImageView{
			get{ return _userImageView;}
			set{ _userImageView = value;}
		}

		public UserCell (NSString cellId):base(UITableViewCellStyle.Default, cellId)
		{

			_userNameLabel = new UILabel
			{
				Font = UIFont.SystemFontOfSize(14),
				TextColor = UIColor.Black,                
				TextAlignment = UITextAlignment.Natural,
				BackgroundColor = UIColor.Clear,
				Lines = 2
			};

			_userImageView = new UIImageView
			{
				BackgroundColor = UIColor.FromRGB(0, 187, 241)
			};

			ContentView.AddSubviews (_userNameLabel, _userImageView);
		}



		public void UpdateCell (string speaker, UIImage image)
		{
			_userImageView.Image = image;
			_userNameLabel.Text = speaker;
		}


		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();

			_userImageView.Frame = new CGRect(ContentView.Bounds.X + 10, ContentView.Bounds.Y + ContentView.Bounds.Height * 0.125f, ContentView.Bounds.Height * 0.75f, ContentView.Bounds.Height* 0.75f);
			_userImageView.Layer.CornerRadius = _userImageView.Frame.Width / 2.0f;
			_userImageView.Layer.MasksToBounds = true;

			_userNameLabel.Frame = new CGRect(_userImageView.Frame.GetMaxX() + 15, 0, ContentView.Frame.Width - (ContentView.Frame.Height + 10), ContentView.Frame.Height);

		}

	}
}

