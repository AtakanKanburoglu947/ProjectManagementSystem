using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystemCore.Dtos;
using ProjectManagementSystemCore.Models;
using ProjectManagementSystemService;
using System;
using System.Linq.Expressions;

namespace ProjectManagementSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RoleController : ControllerBase
    {
        private readonly IService<Role,RoleDto> _roleService;
        public RoleController(IService<Role, RoleDto> roleService) {
            _roleService = roleService;
        }
        [HttpPost]
        public async Task<IActionResult> Add(RoleDto roleDto)
        {
            try
            {
                Role roleExists = await _roleService.Get(x => x.Title == roleDto.Title) ;
                if (roleExists != null)
                {
                    return BadRequest("Rol zaten mevcut");
                }
                else
                {
                    await _roleService.Add(roleDto);
                    return Ok("Rol eklendi");
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
                return Ok(await _roleService.GetAll());
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
                return Ok(await _roleService.Get(id));
            }
            catch (Exception exception)
            {

                return BadRequest(exception.Message);

            }
        }
        [HttpGet("Title")]
        public async Task<IActionResult> Get(string title)
        {
            try
            {
                return Ok(await _roleService.Get(x=>x.Title == title));
            }
            catch (Exception exception)
            {

                return BadRequest(exception.Message);

            }
        }
        [HttpPut]
        public async Task<IActionResult> Update(Role role)
        {
            try
            {
                Expression<Func<Role, bool>> expression = x => x.Title == role.Title;
                await _roleService.Update(role, expression);
                return Ok("Rol güncellendi");
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
                await _roleService.Remove(id);
                return Ok("Rol silindi");
            }
            catch (Exception exception)
            {

                return BadRequest(exception.Message);

            }
        }
        [HttpDelete("Title")]
        public async Task<IActionResult> Remove(string title)
        {
            try
            {
                await _roleService.Remove(x=>x.Title == title);
                return Ok("Rol silindi");
            }
            catch (Exception exception)
            {

                return BadRequest(exception.Message);

            }
        }
    }
}
