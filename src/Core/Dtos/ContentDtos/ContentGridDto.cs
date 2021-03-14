using Core.Dtos.CommonDtos;
using Core.FilterAttributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.ContentDtos
{
    public class ContentGridDto : BaseGridDto
    {
        [Display(Name ="شناسه")]
        [Search(SearchFieldType.Number)]
        public int? Id { get; set; }

        [Display(Name = "عنوان")]
        [Search]
        public string Title { get; set; }

        [Display(Name = "فعال")]
        [Search(SearchFieldType.Boolean)]
        [Boolean]
        public bool? IsActive { get; set; }

        [Display(Name = "تعداد بازدید")]
        public int ViewCount { get; set; }

        [Display(Name = "تاریخ درج")]
        [Search(SearchFieldType.Datetime)]
        public string CreatedOn { get; set; }

        [Display(Name = "آخرین ویرایش")]
        [Search(SearchFieldType.Datetime)]
        public string UpdatedOn { get; set; }

        public int? TagId { get; set; }

        public int? CategoryId { get; set; }
    }
}
