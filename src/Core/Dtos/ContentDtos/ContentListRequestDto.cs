using Core.Models.Enums;

namespace Core.Dtos.ContentDtos
{
    public class ContentListRequestDto
    {
        public string Language { get; set; }
        public ContentType? Type { get; set; }
        public int PageIndex { get; set; } = 1;
        public int? TagId { get; set; }
        public int? CategoryId { get; set; }
    }
}
