using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseApps.Portable.DTO
{
    public class RootObject
    {
        public List<Result> Results { get; set; }
        public string Nationality { get; set; }
        public string Seed { get; set; }
        public string Version { get; set; }
    }
}
