using System.Threading.Tasks;

namespace Core.Interfaces.ContentProviders
{
    public interface IContentCommand
    {
        Task IncreaseViewCountAsync(int contentId);
    }
}
