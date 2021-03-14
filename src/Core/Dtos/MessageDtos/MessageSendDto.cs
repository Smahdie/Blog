using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.MessageDtos
{
    public class MessageSendDto
    {
        [Display(Name = "نام", Prompt = "نام")]
        public string FirstName { get; set; }

        [Display(Name = "نام خانوادگی", Prompt = "نام خانوادگی")]
        public string LastName { get; set; }

        [Display(Name = "ایمیل", Prompt = "ایمیل")]
        [EmailAddress(ErrorMessage = "فرمت ایمیل وارد شده درست نیست.")]
        public string Email { get; set; }

        [Display(Name = "تلفن", Prompt = "تلفن")]
        public string PhoneNumber { get; set; }

        [Display(Name = "متن پیام", Prompt = "متن پیام")]
        [Required(ErrorMessage = "{0} را وارد کنید")]
        public string Text { get; set; }

        [Required(ErrorMessage = "اطلاعات فرم معتبر نیست")]
        public string Token { get; set; }
    }
}
