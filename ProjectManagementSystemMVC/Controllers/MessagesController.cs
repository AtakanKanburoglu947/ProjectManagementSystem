using Auth.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystemCore.Dtos;
using ProjectManagementSystemCore.Models;
using ProjectManagementSystemMVC.Models;
using ProjectManagementSystemService;

namespace ProjectManagementSystemMVC.Controllers
{
    [Authorize]
    public class MessagesController : Controller
    {
        private readonly AuthService _authService;
        private readonly IService<Message,MessageDto,MessageDto> _messageService;
        private readonly NotificationService _notificationService;
        private readonly CacheService _cacheService;
        
        public MessagesController(AuthService authService, IService<Message,MessageDto,MessageDto> messageService, NotificationService notificationService,CacheService cacheService)
        {
            _authService = authService;
            _messageService = messageService;
            _notificationService = notificationService;
            _cacheService = cacheService;
        }
      
        public async Task<IActionResult> Index(int id)
        {
            ViewData["id"] = id;
            if (id > 0)
            {
                id *= 5;
            }
            Guid userIdentityId = await _authService.GetUserIdentityId();
            await _notificationService.Clear(userIdentityId);
            ViewData["notifications"] = await _notificationService.GetNotifications(userIdentityId);
            TimeSpan absoluteExpiration = TimeSpan.FromHours(1);
            TimeSpan slidingExpiration = TimeSpan.FromMinutes(20);
            List<Message> messages = await _cacheService.Get("messages",userIdentityId,absoluteExpiration,slidingExpiration, () => _messageService.Filter(id, x => (DateTime)x.AddedAt!, x => x.ReceiverId == userIdentityId));
            PaginationModel<MessageDetails, NoData> paginationModel = new PaginationModel<MessageDetails, NoData>();
            if (messages != null)
            {
                List<MessageDetails> details = new List<MessageDetails>();
                int count = _messageService.Count(x=>x.ReceiverId == userIdentityId);
                paginationModel.Pagination = new PaginationViewModel() { Count = count };
                
                foreach (var message in messages)
                {
                    var sender = await _authService.GetUserById((Guid)message.SenderId);
                    MessageDetails messageDetails = new MessageDetails()
                    {
                        Id = message.Id,
                        SenderId = message.SenderId,
                        SenderName = sender.UserName,
                        ReceiverId = message.ReceiverId,
                        Name = message.Name,
                        Content = message.Content,
                        AddedAt = message.AddedAt,
                        SenderEmail = sender.Email
                    };
                    details.Add(messageDetails);
                }
                paginationModel.Dataset = details;
            }
            return View(paginationModel);
        }
    }
}
