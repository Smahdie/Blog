using Core.Dtos.MessageDtos;
using Core.Interfaces.MessageProviders;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Admin.Pages.Messages
{
    public class DetailsModel : BasePageModel
    {
        private readonly IMessageManager _messageManager;

        public DetailsModel(IMessageManager messageManager)
        {
            _messageManager = messageManager;
        }

        [BindProperty]
        public MessageDetailsDto Message { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Message = await _messageManager.GetAsync(id);
            if (Message == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
