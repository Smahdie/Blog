using Core.Dtos.CommonDtos;
using Core.Dtos.Settings;
using Core.Dtos.SliderDtos;
using Core.FilterAttributes;
using Core.Helpers;
using Core.Interfaces.SliderProviders;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services.SliderProviders
{
    public class SliderManager : ISliderManager
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly PanelAppSettings _appSettings;        

        public SliderManager(
            ApplicationDbContext dbContext,
            IOptions<PanelAppSettings> appSettings)
        {
            _dbContext = dbContext;
            _appSettings = appSettings.Value;
        }

        public async Task<(List<SliderGridDto> Items, int TotalCount)> GetAllAsync(SliderGridDto dto)
        {
            var query = _dbContext.Sliders.Where(c => !c.Deleted);
            
            if (dto.PageOrder != null)
            {
                query = query.OrderByField(dto.PageOrderBy, dto.PageOrder.ToLower() == "asc");
            }

            query = query.WhereIf(dto.Id.HasValue, c => c.Id == dto.Id);
            query = query.WhereIf(!string.IsNullOrWhiteSpace(dto.Title), c => c.Title.Contains(dto.Title.Trim()));
            query = query.WhereIf(!string.IsNullOrWhiteSpace(dto.Heading), c => c.Heading.Contains(dto.Heading.Trim()));
            query = query.WhereIf(dto.IsActive.HasValue, c => c.IsActive == dto.IsActive);
            if (!string.IsNullOrWhiteSpace(dto.CreatedOn))
            {
                var createdOnDateTime = DateTime.Parse(dto.CreatedOn);
                query = query.Where(c => c.CreatedOn > createdOnDateTime.Date && c.CreatedOn < createdOnDateTime.Date.AddDays(1));
            }
            if (!string.IsNullOrWhiteSpace(dto.UpdatedOn))
            {
                var updatedOnDateTime = DateTime.Parse(dto.UpdatedOn);
                query = query.Where(c => c.UpdatedOn!=null && c.UpdatedOn > updatedOnDateTime.Date && c.UpdatedOn < updatedOnDateTime.Date.AddDays(1));
            }

            var totalCount = query.Count();

            var take = _appSettings.PageSize;
            var skip = (dto.ThisPageIndex - 1) * take;
            query = query.Skip(skip).Take(take);
            var items = await query.Select(c => new SliderGridDto
            {
                Id = c.Id,
                IsActive = c.IsActive,
                Title = c.Title,
                Heading = c.Heading,
                CreatedOn = PersianDateHelper.ConvertToLocalDateTime(c.CreatedOn),
                UpdatedOn = (c.UpdatedOn!=null? PersianDateHelper.ConvertToLocalDateTime(c.UpdatedOn.Value) : "---")
            }).ToListAsync();

            return (items, totalCount);
        }

        public async Task<SliderUpdateDto> GetAsync(int id)
        {
            var slider = await _dbContext.Sliders
                .Where(c => c.Id == id && !c.Deleted)
                .FirstOrDefaultAsync();
            if (slider == null)
            {
                return null;
            }

            return new SliderUpdateDto
            {
                Id = slider.Id,
                Title = slider.Title,
                Heading = slider.Heading,
                Description = slider.Description,
                ImagePath = slider.ImagePath,
                IsActive = slider.IsActive,
                Link = slider.Link,
                LinkText = slider.LinkText
            };
        }

        public async Task<DeleteResultDto> DeleteAsync(int id)
        {
            var slider = await _dbContext.Sliders
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id && !c.Deleted);

            if (slider == null)
            {
                return DeleteResultDto.NotFound(id.ToString());
            }

            try
            {
                slider.Deleted = true;
                slider.DeletedOn = DateTime.Now;

                _dbContext.Update(slider);
                await _dbContext.SaveChangesAsync();
                return DeleteResultDto.Successful(id.ToString());
            }
            catch
            {
                return DeleteResultDto.UnknownError(id.ToString());
            }
        }
        
        public async Task<ChangeStatusResultDto> ChangeStatusAsync(int id)
        {
            var propertyInfo = typeof(SliderGridDto).GetProperty(nameof(SliderGridDto.IsActive));
            if (!propertyInfo.IsDefined(typeof(BooleanAttribute), false))
            {
                return ChangeStatusResultDto.NotPossible(id.ToString());
            }
            var booleanAttribute = propertyInfo.GetCustomAttributes(typeof(BooleanAttribute), false).Cast<BooleanAttribute>().SingleOrDefault();

            var slider = await _dbContext.Sliders
                 .AsNoTracking()
                 .FirstOrDefaultAsync(c => c.Id == id && !c.Deleted);

            if (slider == null)
            {
                return ChangeStatusResultDto.NotFound(id.ToString());
            }

            try
            {
                slider.IsActive = !slider.IsActive;
                slider.UpdatedOn = DateTime.Now;

                _dbContext.Update(slider);
                await _dbContext.SaveChangesAsync();

                if (slider.IsActive)
                {
                    return ChangeStatusResultDto.Successful(id.ToString(), booleanAttribute.TrueBadge, booleanAttribute.TrueText);
                }

                return ChangeStatusResultDto.Successful(id.ToString(), booleanAttribute.FalseBadge, booleanAttribute.FalseText);
            }
            catch
            {
                return ChangeStatusResultDto.UnknownError(id.ToString());
            }
        }
        
        public async Task CreateAsync(SliderCreateDto dto)
        {
            var slider = new Slider
            {
                Title = dto.Title,
                Heading = dto.Heading,
                Description = dto.Description,
                ImagePath = dto.ImagePath,
                Link = dto.Link,
                LinkText = dto.LinkText,
                IsActive = dto.IsActive,
                Language = dto.Language,
                CreatedOn = DateTime.Now
            };
            _dbContext.Sliders.Add(slider);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<CommandResultDto> UpdateAsync(SliderUpdateDto dto)
        {
            var slider = await _dbContext.Sliders.FirstOrDefaultAsync(c => !c.Deleted && c.Id == dto.Id);
            if (slider == null)
            {
                return CommandResultDto.NotFound();
            }

            slider.Title = dto.Title;
            slider.Heading = dto.Heading;
            slider.Description = dto.Description;
            slider.ImagePath = dto.ImagePath;
            slider.Link = dto.Link;
            slider.LinkText = dto.LinkText;
            slider.IsActive = dto.IsActive;
            slider.UpdatedOn = DateTime.Now;
            _dbContext.Update(slider);

            await _dbContext.SaveChangesAsync();

            return CommandResultDto.Successful();

        }
    }
}
