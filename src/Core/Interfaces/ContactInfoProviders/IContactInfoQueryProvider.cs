using Core.Dtos.ContactInfoDtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces.ContactInfoProviders
{
    public interface IContactInfoQueryProvider
    {
        Task<List<ContactInfoListDto>> GetAllAsync();
    }
}
