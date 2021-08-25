using Core.Interfaces.ContentProviders;
using System.Threading.Tasks;

namespace Infrastructure.Services.ContentProviders
{
    public class ContentCommand : IContentCommand
    {
        private readonly ApplicationDbContext _dbContext;
        public ContentCommand(ApplicationDbContext dbContext)
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
