using Core.Interfaces.ContentProviders;
using System.Threading.Tasks;

namespace Infrastructure.Services.ContentProviders
{
    public class ContentCommandProvider : IContentCommandProvider
    {
        private readonly ApplicationDbContext _dbContext;
        public ContentCommandProvider(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task IncreaseViewCountAsync(int contentId)
        {
            var content = await _dbContext.Contents.FindAsync(contentId);
            content.ViewCount++;
            await _dbContext.SaveChangesAsync();
        }
    }
}
