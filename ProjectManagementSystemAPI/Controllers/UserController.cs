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
    public class UserController : ControllerBase
    {
        private readonly IService<User,UserDto,UserDto> _userService;
        private AuthService _authService;
        public UserController(IService<User,UserDto,UserDto> userService, AuthService authService)
        {
            _userService = userService;
            _authService = authService;
        }
        [HttpPost]


        public async Task<IActionResult> Add(UserDto userDto)
        {
            try
            {
                UserIdentity userIdentity = await _authService.GetUserById(userDto.UserIdentityId);
                if (userIdentity != null)
                {
                    User user = await _userService.Get(x=>x.UserIdentityId == userIdentity.Id);
                    if (user == null)
                    {
                        await _userService.Add(userDto);
                        return Ok("Kullanıcı eklendi");

                    }
                    return BadRequest("Kullanıcı zaten mevcut");
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
