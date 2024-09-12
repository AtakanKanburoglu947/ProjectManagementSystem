using ProjectManagementSystemCore.Dtos;
using ProjectManagementSystemCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementSystemService
{
    public class NotificationService
    {
        private readonly IService<User, UserDto, UserUpdateDto> _userService;
        public NotificationService(IService<User,UserDto,UserUpdateDto> userService)
        {
            _userService = userService;
        }
        public async Task Notify(Guid userIdentityId)
        {
            User user = await GetUser(userIdentityId);
            user.Notifications = user.Notifications + 1;
            int notifications = user.Notifications;
            await UpdateUser(user,userIdentityId,notifications);
        }
        public async Task Clear(Guid userIdentityId)
        {
            User user = await GetUser(userIdentityId);
            int notifications = 0;
            await UpdateUser(user, userIdentityId, notifications);

        }
        public async Task<int> GetNotifications(Guid userIdentityId)
        {
            User user = await GetUser(userIdentityId);
            return user.Notifications;
        }
        private async Task<User> GetUser(Guid userIdentityId)
        {
            return await _userService.Get(x => x.UserIdentityId == userIdentityId); ;
        }
        private async Task UpdateUser(User user,Guid userIdentityId, int notifications)
        {
            UserUpdateDto userUpdateDto = new UserUpdateDto()
            {
                Id = user.Id,
                UserIdentityId = userIdentityId,
                Notifications = notifications,
                RoleId = user.RoleId,
            };
            await _userService.Update(userUpdateDto, user.Id);
        }

    }
}
