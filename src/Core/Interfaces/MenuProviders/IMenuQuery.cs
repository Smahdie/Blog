using Core.Dtos.MenuDtos;
using System.Threading.Tasks;

namespace Core.Interfaces.MenuProviders
{
    public interface IMenuQuery
    {
        Task<MenuDetailsDto> GetByKeywordAsync(string keyword, string lang, bool nested = false);
    }
}
