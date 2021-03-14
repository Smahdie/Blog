using Core.Dtos.CommonDtos;
using Core.Dtos.UserDtos;
using Core.Interfaces.UserProviders;
using Core.Models.Manager;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.Pages.Settings
{
    public class ProfileModel : PageModel
    {
        private readonly UserManager<Manager> _userManager;
        private readonly IUserManager _myUserManager;

        public ProfileModel(
            UserManager<Manager> userManager,
            IUserManager myUserManager)
        {
            _userManager = userManager;
            _myUserManager = myUserManager;
        }

        [BindProperty]
        public UserUpdateDto Input { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

            Input = new UserUpdateDto
            {
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Gender = user.Gender
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

            var user = await _userManager.GetUserAsync(User);
            if (user == null) 
            {
                return new JsonResult(CommandResultDto.Failed($"کاربری با شناسه {_userManager.GetUserId(User)} پیدا نشد."));
            }
            var result = await _myUserManager.UpdateMyProfileAsync(user, Input);
            if (result.Success)
            {
                result.Url = Url.Page("/Index");
            }
            
            return new JsonResult(result);
        }
    }
}