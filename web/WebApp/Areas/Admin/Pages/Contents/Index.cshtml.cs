using Canducci.Pagination;
using Core.Dtos.ContentDtos;
using Core.Dtos.Settings;
using Core.Interfaces.ContentProviders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Admin.Pages.Contents
{
    public class IndexModel : BasePageModel
    {
        private readonly IContentManager _contentManager;
        private readonly PanelAppSettings _appSettings;

        public IndexModel(
            IContentManager contentManager,
            IOptions<PanelAppSettings> appSettings)
        {
            _contentManager = contentManager;
            _appSettings = appSettings.Value;
        }

        public StaticPaginated<ContentGridDto> Contents { get; set; }

        public async Task OnGetAsync(int? tagId, int? categoryId)
        {
            var searchDto = new ContentGridDto { TagId = tagId, CategoryId = categoryId };
            var (Items, TotalCount) = await _contentManager.GetAllAsync(searchDto);
            Contents = new StaticPaginated<ContentGridDto>(Items, searchDto.ThisPageIndex, _appSettings.PageSize, TotalCount);
            ViewData["SearchModel"] = searchDto;
        }

        public async Task<IActionResult> OnPostAsync(ContentGridDto searchDto)
        {
            var (Items, TotalCount) = await _contentManager.GetAllAsync(searchDto);
            Contents = new StaticPaginated<ContentGridDto>(Items, searchDto.ThisPageIndex, _appSettings.PageSize, TotalCount);
            ViewData["SearchModel"] = searchDto;
            return PartialView("_Grid", this);
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var result =  await _contentManager.DeleteAsync(id);
            return new JsonResult(result);
        }

        public async Task<IActionResult> OnPostChangeStatusAsync(int id)
        {
            var result = await _contentManager.ChangeStatusAsync(id);
            return new JsonResult(result);
        }
    }
}
