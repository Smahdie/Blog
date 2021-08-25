using Core.Models.Enums;

namespace Core.Dtos.ContentDtos
{
    public class ContentSearchRequestDto
    {
        public string Language { get; set; }
        public ContentType? Type { get; set; }
        public int PageIndex { get; set; } = 1;
        public string Keyword { get; set; }
    }
}
