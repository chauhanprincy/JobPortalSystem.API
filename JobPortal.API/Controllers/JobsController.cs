using JobPortal.API.Data;
using JobPortal.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace JobPortal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public JobsController(ApplicationDbContext context)
        {
            _context = context;
        }

        //Get jobs posted by employer
        [HttpGet("my-jobs")]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> GetEmployerJobs()
        {
            var employerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var jobs = await _context.Jobs
                .Where(j => j.EmployerId == employerId )
                .Select(j => new { 
                    j.Id,
                    j.Title,
                    j.Company,
                    j.Location,
                    j.Description
                })
                .ToListAsync();

            return Ok(jobs);
        }

        //Get All Jobs
        [HttpGet]
        public IActionResult GetJobs() 
        {
            var jobs = _context.Jobs.ToList();
            return Ok(jobs);
        }

        //Create Job (Employer only) API
        [Authorize(Roles = "Employer")]
        [HttpPost]
        public async Task<IActionResult> CreateJob(Job job)
        {

            // Get EmployerId from JWT token
            var employerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            job.CreatedDate = DateTime.Now;
            job.EmployerId = employerId;

            _context.Jobs.Add(job);
            await _context.SaveChangesAsync();

            return Ok("Job created successfully");
        }

        //Shows all jobs on the Homepage 
        [AllowAnonymous]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllJobs()
        {
            var jobs = await _context.Jobs
                .OrderByDescending(j => j.CreatedDate)
                .Select(j => new
                {
                    j.Id,
                    j.Title,
                    j.Company,
                    j.Location,
                    j.Description,
                    j.CreatedDate
                })
                .ToListAsync();

            return Ok(jobs);
        }
    }
}
