using Core.Dtos.CommonDtos;
using Core.Dtos.MessageDtos;
using Core.Dtos.Settings;
using Core.FilterAttributes;
using Core.Helpers;
using Core.Interfaces.MessageProviders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services.MessageProviders
{
    public class MessageManager : IMessageManager
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly PanelAppSettings _appSettings;
        public MessageManager(
            ApplicationDbContext dbContext,
            IOptions<PanelAppSettings> appSettings)
        {
            _dbContext = dbContext;
            _appSettings = appSettings.Value;
        }

        public async Task<(List<MessageGridDto> Items, int TotalCount)> GetAllAsync(MessageGridDto dto)
        {
            var query = _dbContext.Messages.Where(c => !c.Deleted);
            if (dto.PageOrder != null)
            {
                query = query.OrderByField(dto.PageOrderBy, dto.PageOrder.ToLower() == "asc");
            }

            query = query.WhereIf(dto.Id.HasValue, c => c.Id == dto.Id);
            query = query.WhereIf(!string.IsNullOrWhiteSpace(dto.FirstName), c => c.FirstName.Contains(dto.FirstName.Trim()));
            query = query.WhereIf(!string.IsNullOrWhiteSpace(dto.LastName), c => c.LastName.Contains(dto.LastName.Trim()));
            query = query.WhereIf(!string.IsNullOrWhiteSpace(dto.Email), c => c.Email.Contains(dto.Email.Trim()));
            query = query.WhereIf(!string.IsNullOrWhiteSpace(dto.PhoneNumber), c => c.PhoneNumber.Contains(dto.PhoneNumber.Trim()));
            query = query.WhereIf(!string.IsNullOrWhiteSpace(dto.Text), c => c.Text.Contains(dto.Text.Trim()));
            query = query.WhereIf(dto.IsRead.HasValue, c => c.IsRead == dto.IsRead);
            if (!string.IsNullOrWhiteSpace(dto.CreatedOn))
            {
                var createdOnDateTime = DateTime.Parse(dto.CreatedOn);
                query = query.Where(c => c.CreatedOn > createdOnDateTime.Date && c.CreatedOn < createdOnDateTime.Date.AddDays(1));
            }

            var totalCount = query.Count();

            var take = _appSettings.PageSize;
            var skip = (dto.ThisPageIndex - 1) * take;
            query = query.Skip(skip).Take(take);
            var items = await query.Select(a => new MessageGridDto
            {
                Id = a.Id,
                FirstName = a.FirstName,
                LastName = a.LastName,
                Email = a.Email,
                PhoneNumber = a.PhoneNumber,
                Text = a.Text.SubString(50),
                IsRead = a.IsRead,
                CreatedOn = PersianDateHelper.ConvertToLocalDateTime(a.CreatedOn)
            }).ToListAsync();

            return (items, totalCount);
        }

        public async Task<MessageDetailsDto> GetAsync(int id)
        {
            var message = await _dbContext.Messages
               .FirstOrDefaultAsync(c => c.Id == id && !c.Deleted);

            if (message == null)
            {
                return null;
            }

            if (!message.IsRead)
            {
                message.IsRead = true;
                message.UpdatedOn = DateTime.Now;
                await _dbContext.SaveChangesAsync();
            }

            return new MessageDetailsDto
            {
                Id = message.Id,
                FirstName = message.FirstName,
                LastName = message.LastName,
                Email = message.Email,
                PhoneNumber = message.PhoneNumber,
                Text = message.Text,
                IsRead = message.IsRead,
                CreatedOn = PersianDateHelper.ConvertToLocalDateTime(message.CreatedOn)
            };
        }

        public async Task<DeleteResultDto> DeleteAsync(int id)
        {
            var message = await _dbContext.Messages
                 .AsNoTracking()
                 .FirstOrDefaultAsync(c => c.Id == id && !c.Deleted);


            if (message == null)
            {
                return DeleteResultDto.NotFound(id.ToString());
            }

            try
            {
                message.Deleted = true;
                message.DeletedOn = DateTime.Now;

                _dbContext.Update(message);
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
            var propertyInfo = typeof(MessageGridDto).GetProperty(nameof(MessageGridDto.IsRead));
            if (!propertyInfo.IsDefined(typeof(BooleanAttribute), false))
            {
                return ChangeStatusResultDto.NotPossible(id.ToString());
            }
            var booleanAttribute = propertyInfo.GetCustomAttributes(typeof(BooleanAttribute), false).Cast<BooleanAttribute>().SingleOrDefault();

            var message = await _dbContext.Messages
                 .AsNoTracking()
                 .FirstOrDefaultAsync(c => c.Id == id && !c.Deleted);

            if (message == null)
            {
                return ChangeStatusResultDto.NotFound(id.ToString());
            }

            try
            {
                message.IsRead = !message.IsRead;
                message.UpdatedOn = DateTime.Now;

                _dbContext.Update(message);
                await _dbContext.SaveChangesAsync();

                if (message.IsRead)
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
    }
}
