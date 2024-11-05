using Djini_OOP_Lab8.BLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;

namespace Djini_OOP_Lab8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _resumeService;

        public FeedbackController(IFeedbackService resumeService)
        {
            _resumeService = resumeService;
        }

        [HttpGet("{companyId}/{workerId}")]
        public async Task<ActionResult<Feedback>> GetFeedback(int companyId, int workerId)
        {
            try
            {
                var resume = await _resumeService.GetFeedbackAsync(companyId, workerId);
                return resume;
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{companyId}/{workerId}/{resumeContent}")]
        public async Task<ActionResult<Feedback>> CreateFeedback(int companyId, int workerId, string resumeContent)
        {
            try
            {
                var createdFeedback = await _resumeService.CreateFeedbackAsync(companyId, workerId, resumeContent);
                return Ok(createdFeedback);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{companyId}/{workerId}/{resumeContent}")]
        public async Task<IActionResult> UpdateFeedback(int companyId, int workerId, string resumeContent)
        {
            try
            {
                var existingFeedback = await _resumeService.UpdateFeedbackAsync(companyId, workerId, resumeContent);
                return Ok(existingFeedback);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{companyId}/{workerId}")]
        public async Task<IActionResult> DeleteFeedback(int companyId, int workerId)
        {
            try
            {
                var existingFeedback = await _resumeService.DeleteFeedbackAsync(companyId, workerId);
                return Ok(existingFeedback);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

