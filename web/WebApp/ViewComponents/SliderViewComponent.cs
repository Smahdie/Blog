using Core.Interfaces.SliderProviders;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApp.Extensions;

namespace WebApp.ViewComponents
{
    public class SliderViewComponent : ViewComponent
    {
        private readonly ISliderQuery _sliderQueryProvider;

        public SliderViewComponent(ISliderQuery sliderQueryProvider)
        {
            _sliderQueryProvider = sliderQueryProvider;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var language = HttpContext.CurrentLanguage();
            var data = await _sliderQueryProvider.GetListAsync(language);
            return View(data);
        }
    }
}
