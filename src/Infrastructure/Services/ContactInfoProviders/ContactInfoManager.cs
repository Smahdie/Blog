using Core.Dtos.CommonDtos;
using Core.Dtos.ContactInfoDtos;
using Core.Dtos.Settings;
using Core.FilterAttributes;
using Core.Helpers;
using Core.Interfaces.ContactInfoProviders;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services.ContactInfoProviders
{
    public class ContactInfoManager : IContactInfoManager
    {
        private readonly ApplicationDbContext _dbContext;       
        private readonly PanelAppSettings _appSettings;

        public ContactInfoManager(
            ApplicationDbContext dbContext,
            IOptions<PanelAppSettings> appSettings)
        {
            _dbContext = dbContext;
            _appSettings = appSettings.Value;
        }

        public async Task<(List<ContactInfoGridDto> Items, int TotalCount)> GetAllAsync(ContactInfoGridDto dto)
        {
            var query = _dbContext.ContactInfos.Where(c => !c.Deleted);
            if (dto.PageOrder != null)
            {
                query = query.OrderByField(dto.PageOrderBy, dto.PageOrder.ToLower() == "asc");
            }
            query = query.WhereIf(dto.Id.HasValue, c => c.Id == dto.Id);
            query = query.WhereIf(!string.IsNullOrWhiteSpace(dto.Value), c => c.Value.Contains(dto.Value.Trim()));
            query = query.WhereIf(dto.ContactType.HasValue, c => c.ContactType == dto.ContactType);
            query = query.WhereIf(dto.IsActive.HasValue, c => c.IsActive == dto.IsActive);
            if (!string.IsNullOrWhiteSpace(dto.CreatedOn))
            {
                var createdOnDateTime = DateTime.Parse(dto.CreatedOn);
                query = query.Where(c => c.CreatedOn > createdOnDateTime.Date && c.CreatedOn < createdOnDateTime.Date.AddDays(1));
            }
            if (!string.IsNullOrWhiteSpace(dto.UpdatedOn))
            {
                var updatedOnDateTime = DateTime.Parse(dto.UpdatedOn);
                query = query.Where(c => c.UpdatedOn != null && c.UpdatedOn > updatedOnDateTime.Date && c.UpdatedOn < updatedOnDateTime.Date.AddDays(1));
            }

            var totalCount = query.Count();
            var take = _appSettings.PageSize;
            var skip = (dto.ThisPageIndex - 1) * take;
            query = query.Skip(skip).Take(take);
            var items = await query.Select(c => new ContactInfoGridDto
            {
                Id = c.Id,
                IsActive = c.IsActive,
                Value = c.Value,
                ContactType = c.ContactType,
                CreatedOn = PersianDateHelper.ConvertToLocalDateTime(c.CreatedOn),
                UpdatedOn = (c.UpdatedOn != null ? PersianDateHelper.ConvertToLocalDateTime(c.UpdatedOn.Value) : "---")
            }).ToListAsync();

            return (items, totalCount);

        }

        public async Task<ContactInfoUpdateDto> GetAsync(int id)
        {
            var contactInfo = await _dbContext.ContactInfos
                .FirstOrDefaultAsync(c => c.Id == id && !c.Deleted);

            if (contactInfo == null)
            {
                return null;
            }

            return new ContactInfoUpdateDto
            {
                Id = contactInfo.Id,
                Value = contactInfo.Value,
                ContactType = contactInfo.ContactType,
                IsActive = contactInfo.IsActive
            };
        }
       
        public async Task<DeleteResultDto> DeleteAsync(int id)
        {
            var contactInfo = await _dbContext.ContactInfos
               .AsNoTracking()
               .FirstOrDefaultAsync(c => c.Id == id && !c.Deleted);

            if (contactInfo == null)
            {
                return DeleteResultDto.NotFound(id.ToString());
            }

            try
            {
                contactInfo.Deleted = true;
                contactInfo.DeletedOn = DateTime.Now;

                _dbContext.Update(contactInfo);
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
            var propertyInfo = typeof(ContactInfoGridDto).GetProperty(nameof(ContactInfoGridDto.IsActive));
            if (!propertyInfo.IsDefined(typeof(BooleanAttribute), false))
            {
                return ChangeStatusResultDto.NotPossible(id.ToString());
            }
            var booleanAttribute = propertyInfo.GetCustomAttributes(typeof(BooleanAttribute), false).Cast<BooleanAttribute>().SingleOrDefault();

            var contactInfo = await _dbContext.ContactInfos
                 .AsNoTracking()
                 .FirstOrDefaultAsync(c => c.Id == id && !c.Deleted);

            if (contactInfo == null)
            {
                return ChangeStatusResultDto.NotFound(id.ToString());
            }

            try
            {
                contactInfo.IsActive = !contactInfo.IsActive;
                contactInfo.UpdatedOn = DateTime.Now;

                _dbContext.Update(contactInfo);
                await _dbContext.SaveChangesAsync();

                if (contactInfo.IsActive)
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
        
        public async Task CreateAsync(ContactInfoCreateDto dto)
        {
            var contactInfo = new ContactInfo
            {
                Value = dto.Value,
                IsActive = dto.IsActive,
                ContactType = dto.ContactType,
                Language = dto.Language,
                CreatedOn = DateTime.Now
            };

            _dbContext.ContactInfos.Add(contactInfo);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<CommandResultDto> UpdateAsync(ContactInfoUpdateDto dto)
        {
            var contactInfo = await _dbContext.ContactInfos.FirstOrDefaultAsync(c => !c.Deleted && c.Id == dto.Id);
            if (contactInfo == null)
            {
                return CommandResultDto.NotFound();
            }

            contactInfo.Value = dto.Value;
            contactInfo.IsActive = dto.IsActive;
            contactInfo.ContactType = dto.ContactType;
            contactInfo.UpdatedOn = DateTime.Now;

            _dbContext.Update(contactInfo);
            await _dbContext.SaveChangesAsync();
            return CommandResultDto.Successful();
        }
    }
}
