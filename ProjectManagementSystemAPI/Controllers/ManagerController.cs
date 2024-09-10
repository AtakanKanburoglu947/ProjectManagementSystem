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
        private readonly FileService _fileService;
        public ManagerController(IService<Manager,ManagerDto,ManagerUpdateDto> managerService, AuthService authService, FileService fileService)
        {
            _managerService = managerService;
            _authService = authService;
            _fileService = fileService;
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
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(await _managerService.GetAll());
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
                return Ok(await _managerService.Get(id));
            }
            catch (Exception exception)
            {

                return BadRequest(exception.Message);

            }
        }
        [HttpPut]
        public async Task<IActionResult> Update(ManagerUpdateDto managerUpdateDto)
        {
            try
            {
                await _managerService.Update(managerUpdateDto, managerUpdateDto.Id);
                return Ok("Yönetici güncellendi");
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
                await _managerService.Remove(id);
                return Ok("Yönetici silindi");
            }
            catch (Exception exception)
            {

                return BadRequest(exception.Message);

            }
        }
        [HttpGet("Files")]
        public async Task<IActionResult> GetFiles(int id)
        {
            return Ok(await _fileService.GetFilesOfManager(id));
        }


    }
}
