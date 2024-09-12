﻿using Auth.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystemCore.Dtos;
using ProjectManagementSystemCore.Models;
using ProjectManagementSystemMVC.Models;
using ProjectManagementSystemService;

namespace ProjectManagementSystemMVC.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly AuthService _authService;
        private readonly NotificationService _notificationService;

        public AccountController(AuthService authService, NotificationService notificationService)
        {
            _authService = authService;
            _notificationService = notificationService;
        }
        public async Task<IActionResult> Index()
        {
            Guid userIdentityId = await _authService.GetUserIdentityId();
            ViewData["notifications"] = await _notificationService.GetNotifications(userIdentityId);

            UserIdentity userIdentity = await _authService.GetUserById(userIdentityId);
            AccountPageModel accountPageModel = new AccountPageModel()
            {
                Email = userIdentity.Email,
                UserName = userIdentity.UserName,

            };
          
            return View(accountPageModel);
        }

    }
}
