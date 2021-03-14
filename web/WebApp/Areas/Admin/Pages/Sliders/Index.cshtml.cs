using Canducci.Pagination;
using Core.Dtos.Settings;
using Core.Dtos.SliderDtos;
using Core.Interfaces.SliderProviders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Admin.Pages.Sliders
{
    public class IndexModel : BasePageModel
    {
        private readonly ISliderManager _sliderManager;
        private readonly PanelAppSettings _appSettings;

        public IndexModel(
            ISliderManager sliderManager,
            IOptions<PanelAppSettings> appSettings)
        {
            _sliderManager = sliderManager;
            _appSettings = appSettings.Value;
        }

        public StaticPaginated<SliderGridDto> Sliders { get; set; }

        public async Task OnGetAsync()
        {
            var searchDto = new SliderGridDto ();
            var (Items, TotalCount) = await _sliderManager.GetAllAsync(searchDto);
            Sliders = new StaticPaginated<SliderGridDto>(Items, searchDto.ThisPageIndex, _appSettings.PageSize, TotalCount);
            ViewData["SearchModel"] = searchDto;
        }

        public async Task<IActionResult> OnPostAsync(SliderGridDto searchDto)
        {
            var (Items, TotalCount) = await _sliderManager.GetAllAsync(searchDto);
            Sliders = new StaticPaginated<SliderGridDto>(Items, searchDto.ThisPageIndex, _appSettings.PageSize, TotalCount);
            ViewData["SearchModel"] = searchDto;
            return PartialView("_Grid", this);
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var result = await _sliderManager.DeleteAsync(id);
            return new JsonResult(result);
        }

        public async Task<IActionResult> OnPostChangeStatusAsync(int id)
        {
            var result = await _sliderManager.ChangeStatusAsync(id);
            return new JsonResult(result);
        }
    }
}
