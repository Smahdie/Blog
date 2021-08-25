using Core.Dtos.SliderDtos;
using Core.Interfaces.SliderProviders;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services.SliderProviders
{
    public class SliderQuery : ISliderQuery
    {
        private readonly ApplicationDbContext _dbContext;
        public SliderQuery(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<List<SliderListDto>> GetListAsync(string language)
        {
            return Query()
                .Where(a => a.Language == language)
                .Select(a => new SliderListDto
                {
                    Id = a.Id,
                    Heading = a.Heading,
                    Description = a.Description,
                    Link = a.Link,
                    LinkText = a.LinkText,
                    ImagePath = a.ImagePath
                })
                .ToListAsync();
        }

        private IQueryable<Slider> Query()
        {
            return _dbContext.Sliders
                .Where(a => !a.Deleted && a.IsActive);
        }
    }
}