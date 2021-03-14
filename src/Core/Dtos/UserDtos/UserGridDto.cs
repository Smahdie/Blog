using Core.Dtos.CommonDtos;
using Core.FilterAttributes;
using Core.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.UserDtos
{
    public class UserGridDto : BaseGridDto
    {
        [Display(Name = "شناسه")]
        public string Id { get; set; }

        [Search]
        [Display(Name = "نام کاربری")]
        public string UserName { get; set; }

        [Search]
        [Display(Name = "ایمیل")]
        public string Email { get; set; }

        [Search]
        [Display(Name = "تلفن")]
        public string PhoneNumber { get; set; }

        [Search(SearchFieldType.Boolean)]
        [Boolean]
        [Display(Name = "فعال")]
        public bool? IsActive { get; set; }

        [Search(SearchFieldType.Enum)]
        [Display(Name = "جنسیت")]
        public Gender? Gender { get; set; }
    }
}
