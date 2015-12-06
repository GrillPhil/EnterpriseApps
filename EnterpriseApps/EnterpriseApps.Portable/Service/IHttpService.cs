using EnterpriseApps.Portable.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseApps.Portable.Service
{
    public interface IHttpService
    {
        Task<IEnumerable<Result>> GetUsersAsync(int count);
    }
}
