using System.Collections.Generic;
using System.Linq;
using EnterpriseApps.Portable.ExtensionMethods;

namespace EnterpriseApps.Portable.Service
{
    public class MappingService : IMappingService
    {
        public IEnumerable<Model.User> MapUsers(IEnumerable<DTO.User> dtos)
        {
            var models = from dto in dtos
                         select MapUser(dto);

            return models;
        }

        private Model.User MapUser(DTO.User dto)
        {
            var model = new Model.User();

            if (dto == null)
                return null;

            if (dto.Name != null)
            {
                model.FirstName = dto.Name.First.FirstCharToUpper();
                model.LastName = dto.Name.Last.FirstCharToUpper();
            }

            if (dto.Picture != null)
            {
                model.ThumbnailUrl = dto.Picture.Thumbnail;
                model.PictureUrl = dto.Picture.Medium;
            }

            model.Cell = dto.Cell;
            model.Phone = dto.Phone;
            model.Email = dto.Email;

            return model;
        }
    }
}
