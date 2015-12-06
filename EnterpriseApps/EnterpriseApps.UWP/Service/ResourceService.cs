using EnterpriseApps.Portable.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;

namespace EnterpriseApps.UWP.Service
{
    public class ResourceService : IResourceService
    {
        private ResourceLoader _resLoader = new ResourceLoader();

        public string GetString(string key)
        {
            return _resLoader.GetString(key);
        }
    }
}
