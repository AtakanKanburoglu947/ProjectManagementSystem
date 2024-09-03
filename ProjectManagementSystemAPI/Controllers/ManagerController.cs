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
        public ManagerController(IService<Manager,ManagerDto,ManagerUpdateDto> managerService)
        {
            _managerService = managerService;
        }

    }
}
