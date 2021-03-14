using Core.Dtos.CommonDtos;
using Core.FilterAttributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.SliderDtos
{
    public class SliderGridDto : BaseGridDto
    {
        [Display(Name ="شناسه")]
        [Search(SearchFieldType.Number)]
        public int? Id { get; set; }

        [Display(Name = "عنوان")]
        [Search]
        public string Title { get; set; }

        [Display(Name = "تیتر")]
        [Search]
        public string Heading { get; set; }

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
