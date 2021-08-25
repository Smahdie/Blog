using Core.Dtos.PageDtos;
using System.Threading.Tasks;

namespace Core.Interfaces.PageProviders
{
    public interface IPageQuery
    {
        Task<PageDetailsDto> GetDetailsAsync(string keyword, string language);
    }
}
