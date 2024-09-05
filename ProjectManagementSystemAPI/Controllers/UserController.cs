using Auth.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystemCore.Dtos;
using ProjectManagementSystemCore.Models;
using ProjectManagementSystemService;
using System.Linq.Expressions;

namespace ProjectManagementSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IService<User,UserDto, UserUpdateDto> _userService;
        private readonly AuthService _authService;
        private readonly FileService _fileService; 
        public UserController(IService<User,UserDto, UserUpdateDto> userService, AuthService authService, FileService fileService)
        {
            _userService = userService;
            _authService = authService;
            _fileService = fileService;
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
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll() {
            try
            {
                return Ok(await _userService.GetAll("users"));
            }
            catch (Exception exception)
            {

                return BadRequest(exception.Message);

            }
        }

        [HttpGet("Id")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                return Ok(await _userService.Get(id));
            }
            catch (Exception exception)
            {

                return BadRequest(exception.Message);

            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(UserUpdateDto userUpdateDto)
        {
            try
            {
                await _userService.Update(userUpdateDto,userUpdateDto.Id);
                return Ok("Kullanıcı güncellendi");
            }
            catch (Exception exception)
            {

                return BadRequest(exception.Message);

            }
        }



        [HttpDelete("Id")]
        public async Task<IActionResult> Remove(int id)
        {
            try
            {
                await _userService.Remove(id);
                return Ok("Kullanıcı silindi");
            }
            catch (Exception exception)
            {

                return BadRequest(exception.Message);

            }
        }
        [HttpGet("Files")]
        public async Task<IActionResult> GetFiles(Guid id)
        {
            return Ok(await _fileService.GetFilesOfUser(id));
        }




    }
}
