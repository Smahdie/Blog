using Core.Interfaces.SliderProviders;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApp.ViewComponents
{
    public class SliderViewComponent : ViewComponent
    {
        private readonly ISliderQueryProvider _sliderQueryProvider;

        public SliderViewComponent(ISliderQueryProvider sliderQueryProvider)
        {
            _sliderQueryProvider = sliderQueryProvider;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var data = await _sliderQueryProvider.GetListAsync();
            return View(data);
        }
    }
}
