using Core.Dtos.CommonDtos;
using Core.FilterAttributes;
using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.CategoryDtos
{
    public class CategoryGridDto : BaseGridDto
    {
        [Display(Name = "شناسه")]
        [Search(SearchFieldType.Number)]
        public int? Id { get; set; }

        [Display(Name = "نام")]
        [Search]
        public string Name { get; set; }

        [Display(Name = "بالاسری")]
        [Search]
        public string ParentName { get; set; }

        [Display(Name = "فعال")]
        [Boolean]
        [Search(SearchFieldType.Boolean)]
        public bool? IsActive { get; set; }

        [Display(Name = "تاریخ درج")]
        [Search(SearchFieldType.Datetime)]
        public string CreatedOn { get; set; }
    }
}
