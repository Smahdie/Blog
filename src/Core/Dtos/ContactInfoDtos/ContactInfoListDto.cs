using Core.Models.Enums;

namespace Core.Dtos.ContactInfoDtos
{
    public class ContactInfoListDto
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public ContactType ContactType { get; set; }
    }
}
