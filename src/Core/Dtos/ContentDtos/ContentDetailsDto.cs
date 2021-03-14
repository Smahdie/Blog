using Core.Dtos.CategoryDtos;
using Core.Dtos.TagDtos;
using Core.Models.Enums;
using System.Collections.Generic;

namespace Core.Dtos.ContentDtos
{
    public class ContentDetailsDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Summary { get; set; }
        public ContentType Type { get; set; }
        public string Body { get; set; }
        public string ImagePath { get; set; }
        public int ViewCount { get; set; }
        public string PersianCreatedOn { get; set; }
        public string CreatedOn { get; set; }
        public List<TagListDto> Tags { get; set; }
        public List<CategoryListItemDto> Categories { get; set; }
    }
}
