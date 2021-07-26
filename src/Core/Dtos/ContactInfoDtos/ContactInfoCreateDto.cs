using Core.FilterAttributes;
using Core.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.ContactInfoDtos
{
    public class ContactInfoCreateDto
    {
        [Display(Name = "مقدار")]
        [Search]
        public string Value { get; set; }

        [Display(Name = "فعال")]
        public bool IsActive { get; set; }


        [Display(Name = "نوع")]
        [Search(SearchFieldType.Enum)]
        public ContactType ContactType { get; set; }

        [Display(Name = "زبان")]
        public string Language { get; set; }
    }
}
