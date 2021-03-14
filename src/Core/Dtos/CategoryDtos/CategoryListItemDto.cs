using System.Collections.Generic;

namespace Core.Dtos.CategoryDtos
{
    public class CategoryListItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public List<CategoryListItemDto> Children { get; set; }
    }
}
