using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseApps.Portable.Model
{
    public class User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Cell { get; set; }
        public string ThumbnailUrl { get; set; }
        public string PictureUrl { get; set; }
        public string FullName => $"{FirstName} {LastName}";
    }
}
