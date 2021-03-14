using Core.Dtos.CategoryDtos;
using Core.Dtos.PageDtos;
using Core.Models.Enums;

namespace Core.Dtos.MenuDtos
{
    public class MenuMemberDto
    {
        public MenuMemberTargetType TargetType { get; set; }
        public PageListItemDto Page { get; set; }
        public CategoryListItemDto Category { get; set; }
    }
}
