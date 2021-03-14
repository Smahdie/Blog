using Admin.Extensions;
using Core.Dtos.AccountDtos;
using Core.Models.Manager;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Admin.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<Manager> _signInManager;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(SignInManager<Manager> signInManager, ILogger<LoginModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public LoginDto Input { get; set; }

        public string ReturnUrl { get; set; }

        public async Task OnGetAsync(string returnUrl = null)
        {            
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;

            if (!ModelState.IsValid)
            {
                return Page();
            }
            var user = await _signInManager.UserManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "کاربری با این مشخصات وجود ندارد.");
                return Page();
            }

            var result = await _signInManager.PasswordSignInAsync(user, Input.Password, Input.RememberMe, lockoutOnFailure: true);
            
            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in.");
                return LocalRedirect(Url.GetLocalUrl(returnUrl));
            }

            if (result.IsLockedOut)
            {
                _logger.LogWarning("User account locked out.");
                return RedirectToPage("./Lockout");
            }

            ModelState.AddModelError(string.Empty, "نام کاربری یا رمز ورود اشتباه است.");
            return Page();
        }
    }
}
