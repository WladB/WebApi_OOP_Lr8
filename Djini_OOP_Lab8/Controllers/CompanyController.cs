using Djini_OOP_Lab8.BLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Djini_OOP_Lab8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Company>> GetCompany(int id)
        {
            try
            {
                var company = await _companyService.GetCompanyAsync(id);
                return company;
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{caption}")]
        public async Task<ActionResult<Company>> CreateCompany(string caption)
        {
            try
            {
                var createdCompany = await _companyService.CreateCompanyAsync(caption);
                return CreatedAtAction(nameof(GetCompany), new { id = createdCompany.Id }, createdCompany);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPut("{id}/{companyCaption}")]
        public async Task<IActionResult> UpdateCompany(int id, string companyCaption)
        {
            try
            {
                var existingCompany = await _companyService.UpdateCompanyAsync(id, companyCaption);
                return Ok(existingCompany);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
     
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            try
            {
                var existingCompany = await _companyService.DeleteCompanyAsync(id);
                return Ok(existingCompany);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
