namespace Core.Models
{
    public class Content2Tag
    {
        public int ContentId { get; set; }

        public int TagId { get; set; }

        public Content Content { get; set; }

        public Tag Tag { get; set; }
    }
}
