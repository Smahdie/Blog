using Core.Dtos.CommonDtos;
using Core.FilterAttributes;
using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.MenuDtos
{
    public class MenuGridDto : BaseGridDto
    {
        [Display(Name = "شناسه")]
        [Search(SearchFieldType.Number)]
        public int? Id { get; set; }

        [Display(Name = "عنوان")]
        [Search]
        public string Title { get; set; }

        [Display(Name = "کلمه کلیدی")]
        [Search]
        public string Keyword { get; set; }

        [Display(Name = "تاریخ درج")]
        public string CreatedOn { get; set; }

        [Display(Name = "آخرین ویرایش")]
        [Search(SearchFieldType.Datetime)]
        public string UpdatedOn { get; set; }
    }
}
