using Core.Dtos.CommonDtos;
using Core.Dtos.LanguageDtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces.LanguageProviders
{
    public interface ILanguageManager
    {
        Task<(List<LanguageGridDto> Items, int TotalCount)> GetAllAsync(LanguageGridDto dto);
        Task<List<Select2ItemDto>> GetSelectListAsync();
        Task<LanguageUpdateDto> GetAsync(int id);
        Task<ChangeStatusResultDto> ChangeDefaultAsync(int id);
        Task<ChangeStatusResultDto> ChangeActiveAsync(int id);
        Task CreateAsync(LanguageCreateDto dto);
        Task<CommandResultDto> UpdateAsync(LanguageUpdateDto dto);
    }
}
