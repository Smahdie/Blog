using Core.Dtos.CommonDtos;
using Core.FilterAttributes;
using Core.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.ContactInfoDtos
{
    public class ContactInfoGridDto : BaseGridDto
    {
        [Display(Name = "شناسه")]
        [Search(SearchFieldType.Number)]
        public int? Id { get; set; }

        [Display(Name = "مقدار")]
        [Search]
        public string Value { get; set; }

        [Display(Name = "نوع")]
        [Search(SearchFieldType.Enum)]
        public ContactType? ContactType { get; set; }

        [Display(Name = "فعال")]
        [Search(SearchFieldType.Boolean)]
        [Boolean]
        public bool? IsActive { get; set; }

        [Display(Name = "تاریخ درج")]
        [Search(SearchFieldType.Datetime)]
        public string CreatedOn { get; set; }

        [Display(Name = "آخرین ویرایش")]
        [Search(SearchFieldType.Datetime)]
        public string UpdatedOn { get; set; }
    }
}
