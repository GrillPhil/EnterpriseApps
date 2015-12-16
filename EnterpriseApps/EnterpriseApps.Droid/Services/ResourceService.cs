using System;
using EnterpriseApps.Portable.Service;

namespace EnterpriseApps.Droid
{
	public class ResourceService:IResourceService
	{
		public ResourceService ()
		{
		}

		#region IResourceService implementation

		public string GetString (string key)
		{
			return $"[{key}]";
		}

		#endregion
	}
}

