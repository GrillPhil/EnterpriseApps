using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseApps.Portable.Service
{
    public interface IMappingService
    {
        IEnumerable<Model.User> MapUsers(IEnumerable<DTO.Result> dtos);
    }
}
