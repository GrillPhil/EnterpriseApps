using System;
using System.Collections.Generic;
using System.Text;
using EnterpriseApps.Portable.Service;

namespace EnterpriseApps.iOS.Services
{
    class ResourceService:IResourceService
    {
		public ResourceService(){
			Console.WriteLine ("EventApps.iOS || ResourceService || Created");
		}

        public string GetString(string key)
        {
            return "[Not yet implemented]";
        }
    }
}
