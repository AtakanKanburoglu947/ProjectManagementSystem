using Auth.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystemCore.Dtos;
using ProjectManagementSystemCore.Models;
using ProjectManagementSystemService;

namespace ProjectManagementSystemMVC.Controllers
{
    [Authorize]
    public class MessageController : Controller
    {
        private readonly AuthService _authService;
        private readonly IService<Message,MessageDto,MessageDto> _messageService;
        private readonly NotificationService _notificationService;
        private readonly CacheService _cacheService;
        public MessageController(AuthService authService, IService<Message,MessageDto,MessageDto> messageService, NotificationService notificationService, CacheService cacheService)
        {
            _authService = authService;
            _messageService = messageService;
            _notificationService = notificationService;
            _cacheService = cacheService;
        }
        public async Task<IActionResult> Index()
        {
            var senderId = await _authService.GetUserIdentityId();

            ViewData["notifications"] = await _notificationService.GetNotifications(senderId);

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SendMessage(string name,string email, string content)
        {
            var senderId = await _authService.GetUserIdentityId();
            var receiverId = await _authService.GetUserIdentityId(email);
            if (receiverId != Guid.Empty || !string.IsNullOrEmpty(name) || !string.IsNullOrEmpty(email) ||
                !string.IsNullOrEmpty(content))
            {
                MessageDto messageDto = new MessageDto()
                {
                    Name = name,           
                    AddedAt = DateTime.Now,
                    Content = content,
                    ReceiverId = receiverId,
                    SenderId = senderId,
                };
                await _messageService.Add(messageDto);
                _cacheService.SetClass("messages",receiverId, async () => await _messageService.Filter(0, x => (DateTime)x.AddedAt!, x => x.ReceiverId == receiverId), TimeSpan.FromHours(1),TimeSpan.FromMinutes(20));
                await _notificationService.Notify(receiverId);
                TempData["Message"] = "Mesaj gönderildi";
                return Redirect("/Message");

            }
            TempData["Message"] = "Mesaj gönderilemedi";
            return Redirect("/Message");
        }
    }
}
