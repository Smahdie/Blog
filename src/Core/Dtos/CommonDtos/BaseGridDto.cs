namespace Core.Dtos.CommonDtos
{
    public class BaseGridDto
    {
        public int ThisPageIndex { get; set; } = 1;
        
        public string PageOrder { get; set; } = "desc";
        
        public string PageOrderBy { get; set; } = "Id";
    }
}
