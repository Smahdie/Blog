﻿using Core.Interfaces.ContactInfoProviders;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApp.ViewComponents
{
    public class FooterSocialViewComponent : ViewComponent
    {
        private readonly IContactInfoQueryProvider _contactInfoQueryProvider;
        public FooterSocialViewComponent(IContactInfoQueryProvider contactInfoQueryProvider)
        {
            _contactInfoQueryProvider = contactInfoQueryProvider;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var contactInfos = await _contactInfoQueryProvider.GetAllAsync();
            return View(contactInfos);
        }
    }
}
