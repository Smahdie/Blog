using Core.Models.Manager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Admin.Services.Profile
{
    public class ProfileManager
    {
        private readonly UserManager<Manager> _userManager;
        IHttpContextAccessor _httpContextAccessor;

        private Manager _currentUser;

        public ProfileManager(UserManager<Manager> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public Manager CurrentUser
        {
            get
            {
                if (_currentUser == null)
                    _currentUser = _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User).Result;

                return _currentUser;
            }
        }
    }
}
