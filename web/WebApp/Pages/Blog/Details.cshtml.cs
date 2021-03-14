using Core.Dtos.ContentDtos;
using Core.Interfaces.ContentProviders;
using Core.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace WebApp.Pages.Blog
{
    public class DetailsModel : PageModel
    {
        private readonly IContentQueryProvider _contentQueryProvider;
        private readonly IContentCommandProvider _contentCommandProvider;

        public DetailsModel(
            IContentQueryProvider contentQueryProvider,
            IContentCommandProvider contentCommandProvider)
        {
            _contentQueryProvider = contentQueryProvider;
            _contentCommandProvider = contentCommandProvider;
        }

        public ContentDetailsDto Data { get; set; }

        public async Task<IActionResult> OnGet(int id,string slug)        
        {
            var content = await _contentQueryProvider.GetDetailAsync(id, ContentType.Article);
            if(content == null)
            {
                return NotFound();
            }
            if (slug == null || content.Slug != slug)
            {
                return RedirectToPage("Details", new { content.Id, content.Slug });
            }
            Data = content;

            var cookieName = $"blog_visit_{id}";
            var visitedBefore = Request.Cookies[cookieName];
            if(visitedBefore == null)
            {
                await _contentCommandProvider.IncreaseViewCountAsync(id);
                Data.ViewCount++;
            }
            
            return Page();
        }
    }
}
