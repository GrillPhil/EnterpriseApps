using System;
using System.Collections.Generic;
using UIKit;
using EnterpriseApps.Portable.Model;
using Foundation;
using System.Threading.Tasks;

namespace EnterpriseApps.iOS
{
	public class ImageService
	{
		private Dictionary<string, UIImage> _thumbnailDictionary;
		private Dictionary<string, UIImage> _pictureDictionary;

		public ImageService ()
		{
			_thumbnailDictionary = new Dictionary<string, UIImage> ();
			_pictureDictionary = new Dictionary<string, UIImage> ();
		}

		public async Task<UIImage> GetUserThumbnailAsync(User user){
			if (_thumbnailDictionary.ContainsKey (user.Email))
				return _thumbnailDictionary [user.Email];
			if(user.ThumbnailUrl == null)
				return new UIImage();
			
			var image = LoadImage (user.ThumbnailUrl);
			_thumbnailDictionary [user.Email] = image;
			return image;
		}

		public UIImage GetUserPicture (User user){
			if (_pictureDictionary.ContainsKey (user.Email))
				return _pictureDictionary [user.Email];
			if(user.PictureUrl == null)
				return new UIImage();

			var image = LoadImage(user.ThumbnailUrl);
			_pictureDictionary [user.Email] = image;
			return image;
		}

		private UIImage LoadImage(string imageUrl){
			var url = new NSUrl (imageUrl);
			var data = NSData.FromUrl (url);
			var image =  new UIImage (data);
			return image;
		}
	}
}

