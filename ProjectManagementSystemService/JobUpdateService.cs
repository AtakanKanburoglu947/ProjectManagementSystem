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
    public class JobUpdateService
    {
        public AppDbContext _appDbContext;
        public JobUpdateService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task Update(JobDto jobDto, int id)
        {
            Job? job = _appDbContext.Jobs.Find(id);
            if (job != null) {
                job.Id = id;
                job.Title = jobDto.Title;
                job.Description = jobDto.Description;
                job.Status = jobDto.Status;
                job.DueDate = jobDto.DueDate;
                job.UserId = jobDto.UserId;
            }
            await _appDbContext.SaveChangesAsync();
        }
    }
}
