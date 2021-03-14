using Core.Dtos.MenuDtos;
using System.Threading.Tasks;

namespace Core.Interfaces.MenuProviders
{
    public interface IMenuQueryProvider
    {
        Task<MenuDetailsDto> GetByKeywordAsync(string keyword, bool nested = false);
    }
}
