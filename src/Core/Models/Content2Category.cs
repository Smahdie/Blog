namespace Core.Models
{
    public class Content2Category
    {
        public int ContentId { get; set; }

        public int CategoryId { get; set; }

        public Content Content { get; set; }

        public Category Category { get; set; }
    }
}
