using Core.Models.Interfaces;
using System;

namespace Core.Models
{
    public class Page : IAuditable
    {
        public int Id { get; set; }
        public string Keyword { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Body { get; set; }
        public string Language { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool Deleted { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
