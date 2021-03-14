using Core.Dtos.CommonDtos;
using Core.Dtos.UserDtos;
using Core.Interfaces.UserProviders;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.Pages.Users
{
    public class EditModel : BasePageModel
    {
        private readonly IUserManager _userManager;

        public EditModel(IUserManager userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public UserUpdateDto Input { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            Input = await _userManager.GetAsync(id);
            if (Input == null)
            {
                return NotFound();
            }
            
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(m => m.ErrorMessage).ToList();
                return new JsonResult(CommandResultDto.InvalidModelState(errors));
            }

            var result = await _userManager.UpdateAsync(Input);
            if (result.Success)
            {
                result.Url = Url.Page("Index");
            }
            return new JsonResult(result);
        }
    }
}
