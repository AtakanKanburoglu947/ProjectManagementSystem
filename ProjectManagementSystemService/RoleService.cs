using AutoMapper;
using ProjectManagementSystemCore.Dtos;
using ProjectManagementSystemCore.Models;
using ProjectManagementSystemRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementSystemService
{
    public class RoleService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        public RoleService(AppDbContext appDbContext,IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }
        public async Task Add(RoleDto roleDto)
        {
            if (RoleExists(roleDto))
            {
                throw new Exception("Rol zaten mevcut");
            }
            else
            {
                _appDbContext.Roles.Add(_mapper.Map<Role>(roleDto));
                await _appDbContext.SaveChangesAsync();
            }

        }
        private bool RoleExists(RoleDto roleDto)
        {
            return _appDbContext.Roles.FirstOrDefault(r => r.Title == roleDto.Title) != null;
        }
    }
}
