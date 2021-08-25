using Core.Models.Enums;

namespace Core.Dtos.ContentDtos
{
    public class TopContentRequestDto
    {
        public string Language { get; set; }

        public ContentType? Type { get; set; } = null;

        public string PageOrderBy { get; set; } = "Id";

        public string PageOrder { get; set; } = "desc";
    }
}
