using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.CategoryDtos
{
    public class CategoryUpdateDto
    {
        [Display(Name = "شناسه")]
        public int Id { get; set; }

        [Display(Name = "بالاسری")]
        public int? ParentId { get; set; }

        public string ParentName { get; set; }

        [Display(Name = "فعال")]
        public bool IsActive { get; set; }

        public IEnumerable<CategoryTranslationDto> Translations { get; set; }

        public string Names { get; set; }


    }
}
