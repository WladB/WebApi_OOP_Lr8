using Djini_OOP_Lab8.BLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.Design;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Djini_OOP_Lab8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VacanciesController : ControllerBase
    {
        private readonly IVacancyService _vacancyService;

        public VacanciesController(IVacancyService vacancyService)
        {
            _vacancyService = vacancyService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Vacancy>> GetVacancy(int id)
        {
            try
            {
                var vacancy = await _vacancyService.GetVacancyAsync(id);
                return vacancy;
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{caption}/{description}/{companyId}")]
        public async Task<ActionResult<Vacancy>> CreateVacancy(string caption, string description, int companyId)
        {
            try
            {
                var createdVacancy = await _vacancyService.CreateVacancyAsync(caption, description, companyId);
                return CreatedAtAction(nameof(GetVacancy), new { id = createdVacancy.Id }, createdVacancy);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}/{caption}/{description}/{companyId}")]
        public async Task<IActionResult> UpdateVacancy(int id, string caption, string description, int companyId)
        {
            try
            {
                var existingVacancy = await _vacancyService.UpdateVacancyAsync(id, caption, description, companyId);
                return Ok(existingVacancy);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVacancy(int id)
        {
            try
            {
                var existingVacancy = await _vacancyService.DeleteVacancyAsync(id);
                return Ok(existingVacancy);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
