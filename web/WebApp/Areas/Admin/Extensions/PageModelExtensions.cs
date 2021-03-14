using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Admin.Extensions
{
    public static class PageModelExtensions
    {
        public static void SetStatusMessage(this PageModel pageModel, string message)
        {
            pageModel.TempData["StatusMessage"] = message;
        }
    }
}
