using MyFinances.Core.Dtos;
using MyFinances.WebApi.Models.Domains;
using System.Collections.Generic;
using System.Linq;

namespace MyFinances.WebApi.Models.Converters
{
    public static class OperationConverter
    {
        public static OperationDto ToDto(this Operations model)
        {
            return new OperationDto
            {
                CategoryId = model.CategoryId,
                Date = model.Date,
                Description = model.Description,
                Id = model.Id,
                Name = model.Name,
                Value = model.Value
            };
        }

        public static IEnumerable<OperationDto> ToDtos(this IEnumerable<Operations> model)
        {
            if (model == null)
                return [];

            return model.Select(x => x.ToDto());
        }

        public static Operations ToDomain(this OperationDto model)
        {
            return new Operations
            {
                CategoryId = model.CategoryId,
                Date = model.Date,
                Description = model.Description,
                Id = model.Id,
                Name = model.Name,
                Value = model.Value
            };
        }

        public static Operations ToDomain(this OperationCreateDto dto)
        {
            return new Operations
            {
                Name = dto.Name,
                Description = dto.Description,
                Value = dto.Value,
                Date = dto.Date,
                CategoryId = dto.CategoryId
            };
        }

    }
}
