using System;
using System.Collections.Generic;
using Android.Graphics;
using Java.Net;
using EnterpriseApps.Portable.Model;

namespace EnterpriseApps.Droid
{
	public class ImageService
	{
		
			private Dictionary<string, Bitmap> _thumbnailDictionary;
			private Dictionary<string, Bitmap> _pictureDictionary;

			public ImageService ()
			{
				_thumbnailDictionary = new Dictionary<string, Bitmap> ();
				_pictureDictionary = new Dictionary<string, Bitmap> ();
			}

			public  Bitmap GetUserThumbnail(User user){
				if (_thumbnailDictionary.ContainsKey (user.Email))
					return _thumbnailDictionary [user.Email];
				if(user.ThumbnailUrl == null)
					return null;

				var image = LoadImage (user.ThumbnailUrl);
				_thumbnailDictionary [user.Email] = image;
				return image;
			}

			public Bitmap GetUserPicture (User user){
				if (_pictureDictionary.ContainsKey (user.Email))
					return _pictureDictionary [user.Email];
				if(user.PictureUrl == null)
					return null;

				var image = LoadImage(user.PictureUrl);
				_pictureDictionary [user.Email] = image;
				return image;
			}

			private Bitmap LoadImage(string imageUrl){
				var url= new URL (imageUrl);
				var imageBitmap = BitmapFactory.DecodeStream (url.OpenConnection ().InputStream);
				return imageBitmap;
			}
		}

}

