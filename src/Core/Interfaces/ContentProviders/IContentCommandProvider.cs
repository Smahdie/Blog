using System.Threading.Tasks;

namespace Core.Interfaces.ContentProviders
{
    public interface IContentCommandProvider
    {
        Task IncreaseViewCountAsync(int contentId);
    }
}
