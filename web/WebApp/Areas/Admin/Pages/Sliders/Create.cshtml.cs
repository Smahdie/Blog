using Core.Dtos.CommonDtos;
using Core.Dtos.SliderDtos;
using Core.Interfaces.SliderProviders;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.Pages.Sliders
{
    public class CreateModel : BasePageModel
    {
        private readonly ISliderManager _sliderManager;

        public CreateModel(ISliderManager sliderManager)
        {
            _sliderManager = sliderManager;
        }

        [BindProperty]
        public SliderCreateDto Input { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(m => m.ErrorMessage).ToList();
                return new JsonResult(CommandResultDto.InvalidModelState(errors));
            }

            await _sliderManager.CreateAsync(Input);
            var result = CommandResultDto.Successful();
            result.Url = Url.Page("Index");
            return new JsonResult(result);
        }
    }
}
