using Auth.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystemCore.Dtos;
using ProjectManagementSystemCore.Models;
using ProjectManagementSystemService;

namespace ProjectManagementSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagerController : ControllerBase
    {
        private readonly IService<Manager, ManagerDto, ManagerUpdateDto> _managerService;
        private readonly AuthService _authService;
        public ManagerController(IService<Manager,ManagerDto,ManagerUpdateDto> managerService, AuthService authService)
        {
            _managerService = managerService;
            _authService = authService;
        }
        [HttpPost]


        public async Task<IActionResult> Add(ManagerDto managerDto)
        {
            try
            {
                UserIdentity userIdentity = await _authService.GetUserById(managerDto.UserIdentityId);
                if (userIdentity != null)
                {
                    Manager manager = await _managerService.Get(x => x.UserIdentityId == userIdentity.Id);
                    if (manager == null)
                    {
                        await _managerService.Add(managerDto);
                        return Ok("Yönetici eklendi");

                    }
                    return BadRequest("Yönetici zaten mevcut");
                }
                else
                {
                    return BadRequest("Kullanıcı kimliği bulunamadı");
                }
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);

            }
        }


    }
}
