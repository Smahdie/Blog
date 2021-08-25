using Core.Dtos.ContentDtos;
using Core.Interfaces.ContentProviders;
using Core.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApp.Extensions;

namespace WebApp.ViewComponents
{
    public class SidebarArticlesViewComponent : ViewComponent
    {
        private readonly IContentQuery _contentQueryProvider;

        public SidebarArticlesViewComponent(IContentQuery contentQueryProvider)
        {
            _contentQueryProvider = contentQueryProvider;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var language = HttpContext.CurrentLanguage();

            var requestDto = new TopContentRequestDto { Language = language, PageOrderBy = "ViewCount", PageOrder = "desc", Type = ContentType.Article };
            
            var contens = await _contentQueryProvider.GetTopAsync(requestDto);
            
            return View(contens);
        }
    }
}
