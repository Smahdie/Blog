using Core.Dtos.PageDtos;
using System.Threading.Tasks;

namespace Core.Interfaces.PageProviders
{
    public interface IPageQueryProvider
    {
        Task<PageDetailsDto> GetDetailsAsync(string keyword);
    }
}
