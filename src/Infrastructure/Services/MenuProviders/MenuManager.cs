using Core.Dtos.CommonDtos;
using Core.Dtos.MenuDtos;
using Core.Dtos.Settings;
using Core.Helpers;
using Core.Interfaces.MenuProviders;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Infrastructure.Services.MenuProviders
{
    public class MenuManager : IMenuManager
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<MenuManager> _logger;
        private readonly PanelAppSettings _appSettings;

        public MenuManager(
            ApplicationDbContext dbContext,
            ILogger<MenuManager> logger,
            IOptions<PanelAppSettings> appSettings)
        {
            _dbContext = dbContext;
            _logger = logger;
            _appSettings = appSettings.Value;
        }

        public async Task<(List<MenuGridDto> Items, int TotalCount)> GetAllAsync(MenuGridDto dto)
        {
            var query = _dbContext.Menus.AsQueryable();
            if (dto.PageOrder != null)
            {
                query = query.OrderByField(dto.PageOrderBy, dto.PageOrder.ToLower() == "asc");
            }
            query = query.WhereIf(dto.Id.HasValue, c => c.Id == dto.Id);
            query = query.WhereIf(!string.IsNullOrWhiteSpace(dto.Title), c => c.Title.Contains(dto.Title.Trim()));
            query = query.WhereIf(!string.IsNullOrWhiteSpace(dto.Keyword), c => c.Title.Contains(dto.Keyword.Trim()));
            if (!string.IsNullOrWhiteSpace(dto.CreatedOn))
            {
                var createdOnDateTime = DateTime.Parse(dto.CreatedOn);
                query = query.Where(c => c.CreatedOn > createdOnDateTime.Date && c.CreatedOn < createdOnDateTime.Date.AddDays(1));
            }
            if (!string.IsNullOrWhiteSpace(dto.UpdatedOn))
            {
                var UpdatedOnDateTime = DateTime.Parse(dto.UpdatedOn);
                query = query.Where(c => c.UpdatedOn > UpdatedOnDateTime.Date && c.UpdatedOn < UpdatedOnDateTime.Date.AddDays(1));
            }
            var totalCount = query.Count();

            var take = _appSettings.PageSize;
            var skip = (dto.ThisPageIndex - 1) * take;
            query = query.Skip(skip).Take(take);

            var items = await query
                .Select(m => new MenuGridDto
                {
                    Id = m.Id,
                    Title = m.Title,
                    Language = m.Language,
                    Keyword = m.Keyword,
                    CreatedOn = PersianDateHelper.ConvertToLocalDateTime(m.CreatedOn)
                }).ToListAsync();

            return (items, totalCount);
        }

        public async Task<MenuUpdateDto> GetAsync(int id)
        {
            var menu = await _dbContext.Menus
                .Include(c => c.Members).ThenInclude(c => c.Category).ThenInclude(c => c.Translations)
                .Include(c => c.Members).ThenInclude(c => c.Page)
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();

            if (menu == null)
            {
                return null;
            }

            return new MenuUpdateDto
            {
                Id = menu.Id,
                Title = menu.Title,
                Keyword = menu.Keyword,
                PrevMenuMembers = menu.Members
                .OrderBy(c => c.Rank)
                .Select(item => new MenuMemberUpdateDto
                {
                    Id = item.Id,
                    Rank = item.Rank,
                    TargetType = item.TargetType,
                    CategoryId = item.CategoryId,
                    CategoryName = item.CategoryId.HasValue ? item.Category.Translations.First(t => t.Language == "fa").Name : null,
                    PageId = item.PageId,
                    PageTitle = item.PageId.HasValue ? item.Page.Title : null

                }).ToList()
            };
        }

        public async Task<CommandResultDto> CreateAsync(MenuCreateDto dto)
        {
            var menu = new Menu
            {
                Title = dto.Title,
                Keyword = dto.Keyword,
                Language = dto.Language,
                CreatedOn = DateTime.Now
            };

            var dtoMenuItems = JsonSerializer.Deserialize<List<MenuMember>>(dto.MenuMembers);
            if (dtoMenuItems == null || !dtoMenuItems.Any())
            {
                return CommandResultDto.InvalidModelState(new List<string> { "منو باید حداقل یک جز داشته باشد." });
            }

            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                _dbContext.Menus.Add(menu);
                await _dbContext.SaveChangesAsync();

                dtoMenuItems = dtoMenuItems.Select((item, index) =>
                {
                    item.Rank = index + 1;
                    item.MenuId = menu.Id;
                    item.CreatedOn = DateTime.Now;
                    return item;
                }).ToList();

                _dbContext.MenuMembers.AddRange(dtoMenuItems);
                await _dbContext.SaveChangesAsync();

                transaction.Commit();
                return CommandResultDto.Successful();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while creating a new menu");
                return CommandResultDto.UnknownError();
            }
        }

        public async Task<CommandResultDto> UpdateAsync(MenuUpdateDto dto)
        {
            var menu = await _dbContext.Menus
                .Include(m => m.Members)
                 .AsNoTracking()
                 .FirstOrDefaultAsync(c => c.Id == dto.Id);

            if (menu == null)
            {
                return CommandResultDto.NotFound();
            }

            var dtoMenuItems = JsonSerializer.Deserialize<List<MenuMember>>(dto.MenuMembers);
            if (dtoMenuItems == null || !dtoMenuItems.Any())
            {
                return CommandResultDto.InvalidModelState(new List<string> { "منو باید حداقل یک جز داشته باشد." });
            }
            dtoMenuItems = dtoMenuItems.Select((item, index) => { item.Rank = index + 1; return item; }).ToList();

            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                menu.Title = dto.Title;
                menu.UpdatedOn = DateTime.Now;
                _dbContext.Update(menu);

                var currentMenuItems = menu.Members;

                var itemsToAdd = dtoMenuItems.Where(m => m.Id == 0).Select(m=> { m.MenuId = menu.Id; m.CreatedOn = DateTime.Now; return m; });

                var itemsToDelete = currentMenuItems.Where(currentItem => !dtoMenuItems.Any(newItem => newItem.Id == currentItem.Id)).ToList();

                var itemsToUpdate = dtoMenuItems.Where(item => item.Id > 0);

                _dbContext.MenuMembers.AddRange(itemsToAdd);

                _dbContext.MenuMembers.RemoveRange(itemsToDelete);

                foreach (var item in itemsToUpdate)
                {
                    menu.Members.First(m => m.Id == item.Id).Rank = item.Rank;
                }

                await _dbContext.SaveChangesAsync();

                transaction.Commit();
                return CommandResultDto.Successful();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while updating menu");
                return CommandResultDto.UnknownError();
            }
        }
    }
}
