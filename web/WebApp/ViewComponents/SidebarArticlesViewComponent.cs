using Core.Dtos.ContentDtos;
using Core.Interfaces.ContentProviders;
using Core.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApp.ViewComponents
{
    public class SidebarArticlesViewComponent: ViewComponent
    {
        private readonly IContentQueryProvider _contentQueryProvider;
        
        public SidebarArticlesViewComponent(IContentQueryProvider contentQueryProvider)
        {
            _contentQueryProvider = contentQueryProvider;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var requestDto = new TopContentRequestDto { PageOrderBy = "ViewCount", PageOrder = "desc", Type = ContentType.Article };
            var contens = await _contentQueryProvider.GetTopAsync(requestDto);
            return View(contens);
        }
    }
}
