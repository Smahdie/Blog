using Core.Dtos.SliderDtos;
using Core.Interfaces.SliderProviders;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services.SliderProviders
{
    public class SliderQueryProvider : ISliderQueryProvider
    {
        private readonly ApplicationDbContext _dbContext;
        public SliderQueryProvider(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<List<SliderListDto>> GetListAsync()
        {
            return _dbContext.Sliders
                .Where(a => !a.Deleted && a.IsActive)
                .Select(a => new SliderListDto
                {
                    Id = a.Id,
                    Heading = a.Heading,
                    Description = a.Description,
                    Link=a.Link,
                    LinkText  = a.LinkText,
                    ImagePath = a.ImagePath
                })
                .ToListAsync();
        }
    }
}
