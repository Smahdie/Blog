using Canducci.Pagination;
using Core.Dtos.TagDtos;
using Core.Interfaces.TagProviders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Core.Dtos.Settings;

namespace Admin.Pages.Tags
{
    public class IndexModel : BasePageModel
    {
        private readonly ITagManager _tagManager;
        private readonly PanelAppSettings _appSettings;

        public IndexModel(
            ITagManager tagManager,
            IOptions<PanelAppSettings> appSettings)
        {
            _tagManager = tagManager;
            _appSettings = appSettings.Value;
        }

        public StaticPaginated<TagGridDto> Categories { get; set; }

        public async Task OnGetAsync()
        {
            var searchDto = new TagGridDto ();
            var (Items, TotalCount) = await _tagManager.GetAllAsync(searchDto);
            Categories = new StaticPaginated<TagGridDto>(Items, searchDto.ThisPageIndex, _appSettings.PageSize, TotalCount);
            ViewData["SearchModel"] = searchDto;
        }

        public async Task<IActionResult> OnPostAsync(TagGridDto searchDto)
        {
            var (Items, TotalCount) = await _tagManager.GetAllAsync(searchDto);
            Categories = new StaticPaginated<TagGridDto>(Items, searchDto.ThisPageIndex, _appSettings.PageSize, TotalCount);
            ViewData["SearchModel"] = searchDto;
            return PartialView("_Grid", this);
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var result = await _tagManager.DeleteAsync(id);
            return new JsonResult(result);

        }
    }
}
