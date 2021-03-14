namespace Core.Models
{
    public class CategoryTranslation
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Language { get; set; }
        public string Name { get; set; }
        public bool IsDefault { get; set; }
        public Category Category { get; set; }
    }
}
