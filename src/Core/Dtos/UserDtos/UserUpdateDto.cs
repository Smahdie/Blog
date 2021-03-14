using Core.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.UserDtos
{
    public class UserUpdateDto
    {
        [Display(Name = "شناسه", Prompt = "شناسه")]
        public string Id { get; set; }

        [Required(ErrorMessage = "{0} را وارد کنید")]
        [Display(Name = "نام کاربری", Prompt = "نام کاربری")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "{0} را وارد کنید")]
        [EmailAddress(ErrorMessage = "{0} وارد شده معتبر نیست.")]
        [Display(Name = "ایمیل", Prompt = "ایمیل")]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} را مشخص کنید")]
        [Display(Name = "جنسیت", Prompt = "جنسیت")]
        public Gender Gender { get; set; }

        [Display(Name = "تلفن", Prompt = "تلفن")]
        [Phone]
        public string PhoneNumber { get; set; }
    }
}
