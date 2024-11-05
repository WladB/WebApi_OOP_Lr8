using Djini_OOP_Lab8.BLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Djini_OOP_Lab8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkerController : ControllerBase
    {
        private readonly IWorkerService _workerService;

        public WorkerController(IWorkerService workerService)
        {
            _workerService = workerService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Worker>> GetWorker(int id)
        {
            try
            {
                var worker = await _workerService.GetWorkerAsync(id);
                return worker;
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{workerName}/{workerAdress}/{workerPhone}/{workerEmail}")]
        public async Task<ActionResult<Worker>> CreateWorker(string workerName, string workerAdress, string workerPhone, string workerEmail)
        {
            try
            {
                var createdWorker = await _workerService.CreateWorkerAsync(workerName, workerAdress, workerPhone, workerEmail);
                return CreatedAtAction(nameof(GetWorker), new { id = createdWorker.Id }, createdWorker);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}/{workerName}/{workerAdress}/{workerPhone}/{workerEmail}")]
        public async Task<IActionResult> UpdateWorker(int id, string workerName, string workerAdress, string workerPhone, string workerEmail)
        {
            try
            {
                var existingWorker = await _workerService.UpdateWorkerAsync(id, workerName, workerAdress, workerPhone, workerEmail);
                return Ok(existingWorker);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorker(int id)
        {
            try
            {
                var existingWorker = await _workerService.DeleteWorkerAsync(id);
                return Ok(existingWorker);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
