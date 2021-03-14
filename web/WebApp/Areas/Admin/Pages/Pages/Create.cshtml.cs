using Core.Dtos.CommonDtos;
using Core.Dtos.PageDtos;
using Core.Interfaces.PageProviders;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.Pages.Pages
{
    public class CreateModel : BasePageModel
    {
        private readonly IPageManager _pageManager;

        public CreateModel(IPageManager pageManager)
        {
            _pageManager = pageManager;
        }

        [BindProperty]
        public PageCreateDto Input { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(m => m.ErrorMessage).ToList();
                return new JsonResult(CommandResultDto.InvalidModelState(errors));
            }

            await _pageManager.CreateAsync(Input);
            var result = CommandResultDto.Successful();
            result.Url = Url.Page("Index");
            return new JsonResult(result);
        }
    }
}
