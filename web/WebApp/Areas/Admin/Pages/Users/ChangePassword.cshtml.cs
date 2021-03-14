using Core.Dtos.CommonDtos;
using Core.Dtos.UserDtos;
using Core.Interfaces.UserProviders;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.Pages.Users
{
    public class ChangePasswordModel : BasePageModel
    {
        private readonly IUserManager _userManager;

        public ChangePasswordModel(IUserManager userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public UserSetPasswordDto Input { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var user = await _userManager.GetAsync(id);
            if(user == null)
            {
                return NotFound();
            }

            Input = new UserSetPasswordDto
            {
                Id = user.Id,
                Email = user.Email
            };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(m => m.ErrorMessage).ToList();
                return new JsonResult(CommandResultDto.InvalidModelState(errors));
            }

            var result = await _userManager.SetPasswordAsync(Input);
            if (result.Success)
            {
                result.Url = Url.Page("Index");
            }
            return new JsonResult(result);
        }
    }
}
