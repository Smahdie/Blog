using Core.Dtos.CommonDtos;
using Core.Dtos.LanguageDtos;
using Core.Dtos.Settings;
using Core.FilterAttributes;
using Core.Helpers;
using Core.Interfaces.LanguageProviders;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services.LanguageProviders
{
    public class LanguageManager : ILanguageManager
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly PanelAppSettings _appSettings;
        public LanguageManager(
            ApplicationDbContext dbContext,
            IOptions<PanelAppSettings> appSettings)
        {
            _dbContext = dbContext;
            _appSettings = appSettings.Value;
        }

        public async Task<(List<LanguageGridDto> Items, int TotalCount)> GetAllAsync(LanguageGridDto dto)
        {
            var query = _dbContext.Languages.AsQueryable();
            if (dto.PageOrder != null)
            {
                query = query.OrderByField(dto.PageOrderBy, dto.PageOrder.ToLower() == "asc");
            }

            var totalCount = query.Count();

            var take = _appSettings.PageSize;
            var skip = (dto.ThisPageIndex - 1) * take;
            query = query.Skip(skip).Take(take);
            var items = await query.Select(language => new LanguageGridDto
            {
                Id = language.Id,
                IsDefault = language.IsDefault,
                IsActive = language.IsActive,
                Name = language.Name,
                Code = language.Code,
                CreatedOn = PersianDateHelper.ConvertToLocalDateTime(language.CreatedOn)
            }).ToListAsync();

            return (items, totalCount);
        }

        public async Task<LanguageUpdateDto> GetAsync(int id)
        {
            var language = await _dbContext.Languages.FirstOrDefaultAsync(c => c.Id == id);
            if (language == null)
            {
                return null;
            }

            return new LanguageUpdateDto
            {
                Id = language.Id,
                Code = language.Code,
                Name = language.Name
            };
        }

        public async Task<ChangeStatusResultDto> ChangeDefaultAsync(int id)
        {
            var propertyInfo = typeof(LanguageGridDto).GetProperty(nameof(LanguageGridDto.IsDefault));
            if (!propertyInfo.IsDefined(typeof(BooleanAttribute), false))
            {
                return ChangeStatusResultDto.NotPossible(id.ToString());
            }
            var booleanAttribute = propertyInfo.GetCustomAttributes(typeof(BooleanAttribute), false).Cast<BooleanAttribute>().SingleOrDefault();

            var language = await _dbContext.Languages
                 .AsNoTracking()
                 .FirstOrDefaultAsync(c => c.Id == id);

            if (language == null)
            {
                return ChangeStatusResultDto.NotFound(id.ToString());
            }

            if(!language.IsActive && !language.IsDefault)
            {
                return ChangeStatusResultDto.NotPossible(id.ToString());
            }

            try
            {
                var shouldSetNewDefault = language.IsDefault;
                var shouldClearPrevDefault =!language.IsDefault;

                if (shouldSetNewDefault)
                {
                    var setDefaultWasSuccess = await SetNewDefault(language);
                    if (!setDefaultWasSuccess)
                    {
                        return ChangeStatusResultDto.NotPossible(id.ToString());
                    }
                }
                else if(shouldClearPrevDefault) 
                {
                    await ClearPrevDefault(language.Id);
                    language.IsDefault = true; ;
                }

                language.UpdatedOn = DateTime.Now;                

                _dbContext.Update(language);
                await _dbContext.SaveChangesAsync();

                if (language.IsDefault)
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



        public async Task<ChangeStatusResultDto> ChangeActiveAsync(int id)
        {
            var propertyInfo = typeof(LanguageGridDto).GetProperty(nameof(LanguageGridDto.IsDefault));
            if (!propertyInfo.IsDefined(typeof(BooleanAttribute), false))
            {
                return ChangeStatusResultDto.NotPossible(id.ToString());
            }
            var booleanAttribute = propertyInfo.GetCustomAttributes(typeof(BooleanAttribute), false).Cast<BooleanAttribute>().SingleOrDefault();

            var language = await _dbContext.Languages
                 .AsNoTracking()
                 .FirstOrDefaultAsync(c => c.Id == id);

            if (language == null)
            {
                return ChangeStatusResultDto.NotFound(id.ToString());
            }

            try
            {
                var shouldSetNewDefault = language.IsDefault && language.IsActive;
                if (shouldSetNewDefault)
                {
                    var setDefaultWasSuccess = await SetNewDefault(language);
                    if(!setDefaultWasSuccess)
                    {
                        return ChangeStatusResultDto.NotPossible(id.ToString());
                    }
                }

                language.IsActive = !language.IsActive;
                language.UpdatedOn = DateTime.Now;

                _dbContext.Update(language);
                await _dbContext.SaveChangesAsync();

                if (language.IsActive)
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

        public async Task CreateAsync(LanguageCreateDto dto)
        {
            var language = new Language
            {
                Code = dto.Code,
                Name = dto.Name,
                IsDefault = dto.IsDefault,
                CreatedOn = DateTime.Now,
                
            };
            _dbContext.Languages.Add(language);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<CommandResultDto> UpdateAsync(LanguageUpdateDto dto)
        {
            var language = await _dbContext.Languages
                 .AsNoTracking()
                 .FirstOrDefaultAsync(c => c.Id == dto.Id);

            if (language == null)
            {
                return CommandResultDto.NotFound();
            }

            language.Name = dto.Name;
            language.UpdatedOn = DateTime.Now;

            _dbContext.Update(language);
            await _dbContext.SaveChangesAsync();

            return CommandResultDto.Successful();
        }

        private async Task ClearPrevDefault(int languageId)
        {
            var prevDefault = await _dbContext.Languages.AsNoTracking().Where(c => c.IsDefault && c.Id != languageId).FirstOrDefaultAsync();
            if (prevDefault != null)
            {
                prevDefault.IsDefault = false;
                prevDefault.UpdatedOn = DateTime.Now;
                _dbContext.Update(prevDefault);
            }
        }

        private async Task<bool> SetNewDefault(Language language)
        {
            var newDefault = await _dbContext.Languages.AsNoTracking().Where(c => c.IsActive && c.Id != language.Id).FirstOrDefaultAsync();
            if (newDefault == null)
            {
                return false;
            }
            language.IsDefault = false;

            newDefault.IsDefault = true;
            newDefault.UpdatedOn = DateTime.Now;
            _dbContext.Update(newDefault);
            return true;
        }
    }
}
