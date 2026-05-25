using JobPortal.API.Data;
using JobPortal.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using JobPortal.API.DTOs;

namespace JobPortal.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobApplicationController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        //private int UserId;

        public JobApplicationController(ApplicationDbContext context)
        {
            _context = context;
        }

        //JobSeeker can apply for the job
        [Authorize(Roles ="JobSeeker")]
        [HttpPost("apply")]
        public async Task<IActionResult> ApplyJob([FromBody] ApplyJobDto request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int UserId = int.Parse(userIdClaim);

            var application = new JobApplication
            {
                JobId = request.JobId,
                UserId = UserId,
                ApplicationDate = DateTime.UtcNow,
                Status = "Pending"
            };

            _context.JobApplications.Add(application);
            await _context.SaveChangesAsync();  

            return Ok("Application submitted successfully!");
        }

        //JobSeeker can view their applied jobs
        [Authorize(Roles = "JobSeeker")]
        [HttpGet("my-applications")]
        public async Task<IActionResult> GetMyApplication()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var applications = await _context.JobApplications
                .Include (a => a.Job)
                .Where(a => a.UserId == userId)
                .Select(a => new
                {
                    Company = a.Job.Company,
                    JobTitle = a.Job.Title,
                    a.Id,
                    a.JobId,
                    a.ApplicationDate,
                    Status = a.Status
                })

            .ToListAsync();
            return Ok(applications);
        }

        //Employer can view their job applications
        [Authorize(Roles = "Employer")]
        [HttpGet("job/{jobId}/applications")]
        public async Task <IActionResult> GetApplicationsByJob(int jobId)
        {
            // Get EmployerId from JWT Token
            var employerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            // Check if this job belongs to the employer
            var job = await _context.Jobs
                .FirstOrDefaultAsync(j => j.Id == jobId && j.EmployerId == employerId);

            if (job == null)
            {
                return Unauthorized("You can not view applications for this job");
            }

            // Get applications for that job
            var applications = await _context.JobApplications
                .Where(a => a.JobId == jobId)

                .Select(a => new
                {
                    ApplicationId = a.Id,
                    JobId = a.JobId,
                    JobSeekerName = a.User.FullName,
                    Email = a.User.Email,
                    ApplicationDate = a.ApplicationDate,
                })
                .ToListAsync();

            return Ok(applications);
        }

        //Employer can update job status 
        [Authorize(Roles = "Employer")]
        [HttpPut("update-status")]
        public async Task<IActionResult> UpdateApplicationStatus(int applicationId, string status)
        {
            var employerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var application = await _context.JobApplications
                .Include(a => a.Job)
                .FirstOrDefaultAsync(a => a.Id == applicationId);

            if (application == null) 
            {
                return NotFound("Application not found");
            }

            if (application.Job == null) 
            {
                return BadRequest("Job not found for this application");
                Console.WriteLine("EmployerId from token: " + employerId);
                Console.WriteLine("EmployerId from job: " + application.Job.EmployerId);
            }
                
            if (application.Job.EmployerId != applicationId)
                return Unauthorized("You cannot update this application");

            application.Status = status;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Application status update successfully"});   
        }

    }
}
