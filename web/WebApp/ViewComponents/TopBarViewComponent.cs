using Core.Interfaces.ContactInfoProviders;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApp.ViewComponents
{
    public class TopBarViewComponent: ViewComponent
    {
        private readonly IContactInfoQueryProvider _contactInfoQueryProvider;
        public TopBarViewComponent(IContactInfoQueryProvider contactInfoQueryProvider)
        {
            _contactInfoQueryProvider = contactInfoQueryProvider;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = await _contactInfoQueryProvider.GetAllAsync();
            return View(result);
        }
    }
}
