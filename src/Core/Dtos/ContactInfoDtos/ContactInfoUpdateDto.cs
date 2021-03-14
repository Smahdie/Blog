using Core.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.ContactInfoDtos
{
    public class ContactInfoUpdateDto
    {
        [Display(Name = "شناسه")]
        public int Id { get; set; }

        [Display(Name = "مقدار")]
        public string Value { get; set; }

        [Display(Name = "فعال")]
        public bool IsActive { get; set; }

        [Display(Name = "نوع")]
        public ContactType ContactType { get; set; }
    }
}
