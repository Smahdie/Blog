using Core.Interfaces.TagProviders;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApp.ViewComponents
{
    public class TopTagsViewComponent : ViewComponent
    {
        private readonly ITagQueryProvider _tagQueryProvider;
        public TopTagsViewComponent(ITagQueryProvider tagQueryProvider)
        {
            _tagQueryProvider = tagQueryProvider;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var tags = await _tagQueryProvider.GetTopTagsAsync();
            return View(tags);
        }
    }
}
