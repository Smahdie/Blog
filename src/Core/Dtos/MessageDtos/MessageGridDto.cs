using Core.Dtos.CommonDtos;
using Core.FilterAttributes;
using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.MessageDtos
{
    public class MessageGridDto : BaseGridDto
    {
        [Search(SearchFieldType.Number)]
        [Display(Name = "شناسه")]
        public int? Id { get; set; }

        [Search]
        [Display(Name = "نام")]
        public string FirstName { get; set; }

        [Search]
        [Display(Name = "نام خانوادگی")]
        public string LastName { get; set; }

        [Search]
        [Display(Name = "ایمیل")]
        public string Email { get; set; }

        [Search]
        [Display(Name = "تلفن")]
        public string PhoneNumber { get; set; }

        [Search]
        [Display(Name = "متن پیام")]
        public string Text { get; set; }

        [Search(SearchFieldType.Datetime)]
        [Display(Name = "تاریخ ارسال")]
        public string CreatedOn { get; set; }

        [Search(SearchFieldType.Boolean)]
        [Boolean("خوانده شده", "خوانده نشده", "badge-secondary", "badge-warning")]
        [Display(Name = "وضعیت")]
        public bool? IsRead { get; set; }
    }
}
