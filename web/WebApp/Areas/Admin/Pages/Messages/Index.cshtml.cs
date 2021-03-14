using Canducci.Pagination;
using Core.Dtos.MessageDtos;
using Core.Dtos.Settings;
using Core.Interfaces.MessageProviders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Admin.Pages.Messages
{
    public class IndexModel : BasePageModel
    {
        private readonly IMessageManager _messageManager;
        private readonly PanelAppSettings _appSettings;

        public IndexModel(
            IMessageManager messageManager,
            IOptions<PanelAppSettings> appSettings)
        {
            _messageManager = messageManager;
            _appSettings = appSettings.Value;
        }

        public StaticPaginated<MessageGridDto> Messages { get; set; }

        public async Task OnGetAsync(bool? isRead)
        {
            var searchDto = new MessageGridDto { IsRead = isRead };
            var (Items, TotalCount) = await _messageManager.GetAllAsync(searchDto);
            Messages = new StaticPaginated<MessageGridDto>(Items, searchDto.ThisPageIndex, _appSettings.PageSize, TotalCount);
            ViewData["SearchModel"] = searchDto;
        }

        public async Task<IActionResult> OnPostAsync(MessageGridDto searchDto)
        {
            var (Items, TotalCount) = await _messageManager.GetAllAsync(searchDto);
            Messages = new StaticPaginated<MessageGridDto>(Items, searchDto.ThisPageIndex, _appSettings.PageSize, TotalCount);
            ViewData["SearchModel"] = searchDto;
            return PartialView("_Grid", this);
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var result = await _messageManager.DeleteAsync(id);
            return new JsonResult(result);

        }

        public async Task<IActionResult> OnPostChangeStatusAsync(int id)
        {
            var result = await _messageManager.ChangeStatusAsync(id);
            return new JsonResult(result);
        }
    }
}
