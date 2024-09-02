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
        public async Task Update(JobUpdateDto jobUpdateDto)
        {
            Job? job = _appDbContext.Jobs.Find(jobUpdateDto.Id);
            if (job != null) {
                job.Id = jobUpdateDto.Id;
                job.Title = jobUpdateDto.Title;
                job.Description = jobUpdateDto.Description;
                job.Status = jobUpdateDto.Status;
                job.DueDate = jobUpdateDto.DueDate;
                job.UserId = jobUpdateDto.UserId;
            }
            await _appDbContext.SaveChangesAsync();
        }
    }
}
