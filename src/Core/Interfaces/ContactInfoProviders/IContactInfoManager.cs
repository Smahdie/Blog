using Core.Dtos.CommonDtos;
using Core.Dtos.ContactInfoDtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces.ContactInfoProviders
{
    public interface IContactInfoManager
    {
        Task<(List<ContactInfoGridDto> Items, int TotalCount)> GetAllAsync(ContactInfoGridDto dto);
        Task<ContactInfoUpdateDto> GetAsync(int id);
        Task<DeleteResultDto> DeleteAsync(int id);
        Task<ChangeStatusResultDto> ChangeStatusAsync(int id);
        Task CreateAsync(ContactInfoCreateDto dto);
        Task<CommandResultDto> UpdateAsync(ContactInfoUpdateDto dto);
    }
}
