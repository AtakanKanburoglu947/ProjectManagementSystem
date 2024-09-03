using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
    public class ProjectService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        public ProjectService(AppDbContext appDbContext, IMapper mapper) {
            _appDbContext = appDbContext; 
            _mapper = mapper;
        }
        public async Task AddManager(Guid projectId, int managerId)
        {
             


        }

        public async Task<List<User>> GetUsers(Guid id)
        {
            Project project = await _appDbContext.Projects.FindAsync(id);
            if (project != null)
            {
                return project.Users.ToList();
            }
            return null;
        }
    }
}
