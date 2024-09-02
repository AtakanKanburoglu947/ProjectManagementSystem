﻿using Auth;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    [RoleAuthorize(["Admin"])]
    public class JobController : ControllerBase
    {
        private readonly IService<Job,JobDto,JobUpdateDto> _jobService;
        public JobController(IService<Job,JobDto,JobUpdateDto> jobService)
        {
            _jobService = jobService;
        }
        [HttpPost]
     

        public async Task<IActionResult> Add(JobDto jobDto)
        {
            try
            {
                Job jobExists = await _jobService.Get(x => x.Title == jobDto.Title);
                if (jobExists != null)
                {
                    return BadRequest("İş zaten mevcut");
                }
                else
                {
                    await _jobService.Add(jobDto);
                    return Ok("İş eklendi");
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
                return Ok(await _jobService.GetAll());
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
                return Ok(await _jobService.Get(id));
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
                return Ok(await _jobService.Get(x => x.Title == title));
            }
            catch (Exception exception)
            {

                return BadRequest(exception.Message);

            }
        }
        [HttpPut]

        public async Task<IActionResult> Update(JobUpdateDto jobUpdateDto)
        {
            try
            {
                await _jobService.Update(jobUpdateDto,jobUpdateDto.Id);
                return Ok("İş güncellendi");
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
                await _jobService.Remove(id);
                return Ok("İş silindi");
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
                await _jobService.Remove(x => x.Title == title);
                return Ok("İş silindi");
            }
            catch (Exception exception)
            {

                return BadRequest(exception.Message);

            }
        }
    }
}
