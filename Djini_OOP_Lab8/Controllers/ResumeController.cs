using Djini_OOP_Lab8.BLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Djini_OOP_Lab8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResumeController: ControllerBase
    {
        private readonly IResumeService _resumeService;

        public ResumeController(IResumeService resumeService)
        {
            _resumeService = resumeService;
        }

        [HttpGet("{vacancyId}/{workerId}")]
        public async Task<ActionResult<Resume>> GetResume(int vacancyId, int workerId)
        {
            try
            {
                var resume = await _resumeService.GetResumeAsync(vacancyId, workerId);
                return resume;
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{vacancyId}/{workerId}/{resumeContent}")]
        public async Task<ActionResult<Resume>> CreateResume(int vacancyId, int workerId, string resumeContent)
        {
            try
            {
                var createdResume = await _resumeService.CreateResumeAsync(vacancyId, workerId, resumeContent);
                return  Ok(createdResume);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{vacancyId}/{workerId}/{resumeContent}")]
        public async Task<IActionResult> UpdateResume(int vacancyId, int workerId, string resumeContent)
        {
            try
            {
                var existingResume = await _resumeService.UpdateResumeAsync(vacancyId, workerId, resumeContent);
                return Ok(existingResume);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{vacancyId}/{workerId}")]
        public async Task<IActionResult> DeleteResume(int vacancyId, int workerId)
        {
            try
            {
                var existingResume = await _resumeService.DeleteResumeAsync(vacancyId, workerId);
                return Ok(existingResume);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}