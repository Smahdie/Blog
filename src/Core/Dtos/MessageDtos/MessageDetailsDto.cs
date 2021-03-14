using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.MessageDtos
{
    public class MessageDetailsDto
    {
        public int Id { get; set; }

        [Display(Name = "نام")]
        public string FirstName { get; set; }

        [Display(Name = "نام خانوادگی")]
        public string LastName { get; set; }

        [Display(Name = "ایمیل")]
        public string Email { get; set; }

        [Display(Name = "تلفن")]
        public string PhoneNumber { get; set; }

        [Display(Name = "پیام")]
        public string Text { get; set; }

        [Display(Name = "خوانده شده")]
        public bool IsRead { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        public string CreatedOn { get; set; }
    }
}