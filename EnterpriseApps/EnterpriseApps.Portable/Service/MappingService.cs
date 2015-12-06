using System.Collections.Generic;
using System.Linq;
using EnterpriseApps.Portable.ExtensionMethods;

namespace EnterpriseApps.Portable.Service
{
    public class MappingService : IMappingService
    {
        public IEnumerable<Model.User> MapUsers(IEnumerable<DTO.Result> dtos)
        {
            var models = from dto in dtos
                         select MapUser(dto);

            return models;
        }

        private Model.User MapUser(DTO.Result dto)
        {
            var model = new Model.User();

            if (dto == null || dto.User == null)
                return null;

            if (dto.User.Name != null)
            {
                model.FirstName = dto.User.Name.First.FirstCharToUpper();
                model.LastName = dto.User.Name.Last.FirstCharToUpper();
            }

            if (dto.User.Picture != null)
            {
                model.ThumbnailUrl = dto.User.Picture.Thumbnail;
                model.PictureUrl = dto.User.Picture.Large;
            }

            model.Cell = dto.User.Cell;
            model.Phone = dto.User.Phone;
            model.Email = dto.User.Email;

            return model;
        }
    }
}
