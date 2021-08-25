using Core.Interfaces.ContactInfoProviders;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApp.Extensions;

namespace WebApp.ViewComponents
{
    public class FooterContactViewComponent: ViewComponent
    {
        private readonly IContactInfoQuery _contactInfoQueryProvider;
        public FooterContactViewComponent(IContactInfoQuery contactInfoQueryProvider)
        {
            _contactInfoQueryProvider = contactInfoQueryProvider;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var language = HttpContext.CurrentLanguage();
            var contactInfos = await _contactInfoQueryProvider.GetAllAsync(language);
            return View(contactInfos);
        }
    }
}
