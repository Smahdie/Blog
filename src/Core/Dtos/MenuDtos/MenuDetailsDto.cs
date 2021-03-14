using System.Collections.Generic;

namespace Core.Dtos.MenuDtos
{
    public class MenuDetailsDto
    {
        public string Title { get; set; }
        public List<MenuMemberDto> MenuMembers { get; set; }
    }
}
