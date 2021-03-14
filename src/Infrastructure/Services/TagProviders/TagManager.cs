using Core.Dtos.CommonDtos;
using Core.Dtos.Settings;
using Core.Dtos.TagDtos;
using Core.Helpers;
using Core.Interfaces.TagProviders;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services.TagProviders
{
    public class TagManager : ITagManager
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<TagManager> _logger;
        private readonly PanelAppSettings _appSettings;
        public TagManager(
            ApplicationDbContext dbContext,
            ILogger<TagManager> logger,
            IOptions<PanelAppSettings> appSettings)
        {
            _dbContext = dbContext;
            _logger = logger;
            _appSettings = appSettings.Value;
        }

        public async Task<(List<TagGridDto> Items, int TotalCount)> GetAllAsync(TagGridDto dto)
        {
            var query = _dbContext.Tags.Where(c => !c.Deleted);
            if (dto.PageOrder != null)
            {
                query = query.OrderByField(dto.PageOrderBy, dto.PageOrder.ToLower() == "asc");
            }

            query = query.WhereIf(dto.Id.HasValue, c => c.Id == dto.Id);
            query = query.WhereIf(!string.IsNullOrWhiteSpace(dto.Name), c => c.Name.Contains(dto.Name.Trim()));
            if (!string.IsNullOrWhiteSpace(dto.CreatedOn))
            {
                var createdOnDateTime = DateTime.Parse(dto.CreatedOn);
                query = query.Where(c => c.CreatedOn > createdOnDateTime.Date && c.CreatedOn < createdOnDateTime.Date.AddDays(1));
            }

            var totalCount = query.Count();

            var take = _appSettings.PageSize;
            var skip = (dto.ThisPageIndex - 1) * take;
            query = query.Skip(skip).Take(take);
            var items = await query.Select(tag => new TagGridDto
            {
                Id = tag.Id,
                Name = tag.Name,
                CreatedOn = PersianDateHelper.ConvertToLocalDateTime(tag.CreatedOn)
            }).ToListAsync();

            return (items, totalCount);
        }

        public async Task<Select2PagedResult> GetSelectListAsync(string term, int page)
        {
            var take = _appSettings.PageSize;
            var takePlusOne = take + 1;
            var skip = (page - 1) * take;
            
            var tags = await _dbContext.Tags
                .Where(t => !t.Deleted)
                .Where(t => t.Name.Contains(term.Trim()))
                .Skip(skip)
                .Take(takePlusOne)
                .Select(t => new Select2ItemDto 
                {
                    Id = t.Id,
                    Text = t.Name
                })
                .ToListAsync();

            var result = new Select2PagedResult();
            if (tags.Count > take)
                result.Pagination.More = true;
            result.Results = tags.Take(take).ToList();
            return result;
        }

        public async Task<List<int>> CreateAsync(IEnumerable<string> words)
        {
            var result = new List<int>();
            var tags = words.Select(word => new Tag 
            {
                Name = word.Trim(),
                CreatedOn = DateTime.Now
            });
            foreach (var tag in tags)
            {
                _dbContext.Tags.Add(tag);
                await _dbContext.SaveChangesAsync();
                result.Add(tag.Id);
            }

            return result;
        }

        public async Task<DeleteResultDto> DeleteAsync(int id)
        {
            var tag = await _dbContext.Tags
              .AsNoTracking()
              .FirstOrDefaultAsync(c => c.Id == id && !c.Deleted);

            if (tag == null)
            {
                return DeleteResultDto.NotFound(id.ToString());
            }

            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                tag.Deleted = true;
                tag.DeletedOn = DateTime.Now;
                _dbContext.Update(tag);

                _dbContext.Content2Tags.RemoveRange(_dbContext.Content2Tags.Where(t => t.TagId == id));

                await _dbContext.SaveChangesAsync();

                transaction.Commit();
                return DeleteResultDto.Successful(id.ToString());
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Unable to delete tag");
                return DeleteResultDto.UnknownError(id.ToString());
            }
        }
    }
}
