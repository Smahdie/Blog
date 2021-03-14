using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Admin.Pages
{
    public class BasePageModel : PageModel
    {
        [NonAction]
        public virtual PartialViewResult PartialView(string viewName, object model)
        {
            ViewData.Model = model;

            return new PartialViewResult()
            {
                ViewName = viewName,
                ViewData = ViewData,
                TempData = TempData
            };
        }
    }
}
