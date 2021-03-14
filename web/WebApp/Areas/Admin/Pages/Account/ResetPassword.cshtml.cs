using Core.Dtos.AccountDtos;
using Core.Models.Manager;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;

namespace Admin.Pages.Account
{
    public class ResetPasswordModel : PageModel
    {
        private readonly UserManager<Manager> _userManager;

        public ResetPasswordModel(UserManager<Manager> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public ResetPasswordDto Input { get; set; }

        public IActionResult OnGet(string code = null)
        {
            if (code == null)
            {
                throw new ApplicationException("A code must be supplied for password reset.");
            }
            else
            {
                Input = new ResetPasswordDto { Code = code };
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToPage("./ResetPasswordConfirmation");
            }

            var result = await _userManager.ResetPasswordAsync(user, Input.Code, Input.Password);
            if (result.Succeeded)
            {
                return RedirectToPage("./ResetPasswordConfirmation");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page();
        }
    }
}
