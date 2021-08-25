using Core.Interfaces.TagProviders;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApp.Extensions;

namespace WebApp.ViewComponents
{
    public class TopTagsViewComponent : ViewComponent
    {
        private readonly ITagQuery _tagQueryProvider;


        public TopTagsViewComponent(ITagQuery tagQueryProvider)
        {
            _tagQueryProvider = tagQueryProvider;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var language = HttpContext.CurrentLanguage();
            var tags = await _tagQueryProvider.GetTopTagsAsync(language);
            return View(tags);
        }
    }
}
