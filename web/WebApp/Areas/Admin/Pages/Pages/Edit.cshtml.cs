using Core.Dtos.PageDtos;
using Core.Dtos.CommonDtos;
using Core.Interfaces.PageProviders;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;

namespace Admin.Pages.Pages
{
    public class EditModel : BasePageModel
    {
        private readonly IPageManager _pageManager;

        public EditModel(IPageManager pageManager)
        {
            _pageManager = pageManager;
        }

        [BindProperty]
        public PageUpdateDto Input { get; set; }

        public async Task OnGetAsync(int id)
        {
            Input = await _pageManager.GetAsync(id);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(m => m.ErrorMessage).ToList();
                return new JsonResult(CommandResultDto.InvalidModelState(errors));
            }

            await _pageManager.UpdateAsync(Input);
            var result = CommandResultDto.Successful();
            result.Url = Url.Page("Index");
            return new JsonResult(result);
        }
    }
}
