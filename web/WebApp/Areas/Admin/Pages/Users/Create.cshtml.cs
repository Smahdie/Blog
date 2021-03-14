using Core.Dtos.CommonDtos;
using Core.Dtos.UserDtos;
using Core.Interfaces.UserProviders;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.Pages.Users
{
    public class CreateModel : BasePageModel
    {
        private readonly IUserManager _userManager;

        public CreateModel(IUserManager userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public UserCreateDto Input { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(m => m.ErrorMessage).ToList();
                return new JsonResult(CommandResultDto.InvalidModelState(errors));
            }

            var result = await _userManager.CreateAsync(Input);
            if (result.Success)
            {
                result.Url = Url.Page("Index");
            }
            return new JsonResult(result);
        }
    }
}
