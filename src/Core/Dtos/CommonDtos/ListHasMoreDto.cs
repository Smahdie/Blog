using System.Collections.Generic;

namespace Core.Dtos.CommonDtos
{
    public class ListHasMoreDto<T>
        where T:class
    {
        public List<T> Items { get; set; }
        public bool HasMore { get; set; }
    }
}
