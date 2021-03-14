using Core.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.UserDtos
{
    public class UserCreateDto
    {
        [Required(ErrorMessage = "{0} را وارد کنید")]
        [Display(Name = "نام کاربری", Prompt = "نام کاربری")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "{0} را وارد کنید")]
        [EmailAddress(ErrorMessage = "{0} وارد شده معتبر نیست.")]
        [Display(Name = "ایمیل", Prompt = "ایمیل")]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} را وارد کنید")]
        [StringLength(100, ErrorMessage = "طول {0} باید حداقل {2} و حداکثر {1} کاراکتر باشد.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "رمز عبور", Prompt = "رمز عبور")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "تکرار رمز عبور", Prompt = "تکرار رمز عبور")]
        [Compare("Password", ErrorMessage = "رمز عبور و تکرار آن برابر نیستند")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "{0} را مشخص کنید")]
        [Display(Name = "جنسیت", Prompt = "جنسیت")]
        public Gender Gender { get; set; }

        [Display(Name = "تلفن", Prompt = "تلفن")]
        [Phone]
        public string PhoneNumber { get; set; }
    }
}
