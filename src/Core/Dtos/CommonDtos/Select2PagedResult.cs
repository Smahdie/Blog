using System.Collections.Generic;

namespace Core.Dtos.CommonDtos
{
    public class Select2PagedResult
    {
        public Select2PagedResult()
        {
            Pagination = new Select2Pagination { More = false };
        }
        public List<Select2ItemDto> Results { get; set; }
        public Select2Pagination Pagination { get; set; }
    }
}
