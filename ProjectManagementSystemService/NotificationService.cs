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
        private readonly IService<Manager, ManagerDto, ManagerUpdateDto> _managerService;
        public NotificationService(IService<User,UserDto,UserUpdateDto> userService, IService<Manager,ManagerDto,ManagerUpdateDto> managerService)
        {
            _userService = userService;
            _managerService = managerService;
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
            int notifications = 0;
            User? user = await GetUser(userIdentityId)!;
            if (user !=null)
            {
                await UpdateUser(user, userIdentityId, notifications);
                
            }
            Manager? manager  = await GetManager(userIdentityId)!;
            if (manager != null)
            {
                await UpdateManager(manager, userIdentityId, notifications);
            }

        }
        public async Task<int> GetNotifications(Guid userIdentityId)
        {
            User? user = await GetUser(userIdentityId)!;
            if (user != null)
            {
                return user.Notifications;
            
            }
            Manager? manager = await GetManager(userIdentityId)!;
            if (manager != null)
            {
                return manager.Notifications;
            }
            return 0;
        }
        private async Task<User>? GetUser(Guid userIdentityId)
        {
            User user = await _userService.Get(x => x.UserIdentityId == userIdentityId);
            if (user != null)
            {
                return user;
            }
            return null!;
        }
        private async Task<Manager>? GetManager(Guid userIdentityId)
        {
            Manager manager = await _managerService.Get(x=>x.UserIdentityId==userIdentityId);
            if (manager != null)
            {
                return manager;
            }
            return null!;
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
        private async Task UpdateManager(Manager manager,Guid userIdentityId, int notifications)
        {
            ManagerUpdateDto managerUpdateDto = new ManagerUpdateDto()
            {
                Id = manager.Id,
                UserIdentityId=userIdentityId,
                RoleId= manager.RoleId,
                Notifications=notifications,
            };
            await _managerService.Update(managerUpdateDto,manager.Id);
        }

    }
}
