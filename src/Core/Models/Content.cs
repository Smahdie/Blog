using Core.Models.Enums;
using Core.Models.Interfaces;
using System;
using System.Collections.Generic;


namespace Core.Models
{
    public class Content : IAuditable
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public ContentType Type { get; set; }
        public string Body { get; set; }
        public string ImagePath { get; set; }
        public bool IsActive { get; set; }
        public int ViewCount { get; set; }
        public string Language { get; set; }
        public bool Deleted { get; set; }
        public DateTime? DeletedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public DateTime CreatedOn { get; set; }
        public ICollection<Content2Category> Categories { get; set; }
        public ICollection<Content2Tag> Tags { get; set; }
    }
}
