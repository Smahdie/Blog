using Core.Dtos.CommonDtos;
using Core.FilterAttributes;
using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.TagDtos
{
    public class TagGridDto : BaseGridDto
    {
        [Display(Name = "شناسه")]
        [Search]
        public int? Id { get; set; }

        [Display(Name = "نام")]
        [Search]
        public string Name { get; set; }

        [Display(Name = "تاریخ درج")]
        [Search(SearchFieldType.Datetime)]
        public string CreatedOn { get; set; }
    }
}
