using Djini_OOP_Lab8.BLL;
using Microsoft.AspNetCore.Mvc;

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

        /// <summary>
        /// Отримує компанію за її ідентифікатором
        /// </summary>
        /// <param name="id">Ідентифікатор компанії</param>
        /// <returns>
        /// Повертає об'єкт компанії, якщо знайдено; у разі відсутності або виникнення помилки повертає BadRequest з повідомленням про помилку.
        /// </returns>
        [HttpGet("{id}")]
        [ProducesResponseType (typeof(Company), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

        /// <summary>
        /// Створює нову компанію
        /// </summary>
        /// <param name="caption">Назва компанії</param>
        /// <returns>
        /// Повертає рядок, що містить створений об'єкт company, якщо знайдено відповідний запис; у разі відсутності або виникнення помилки повертає BadRequest з повідомленням про помилку
        /// </returns>
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("{caption}")]
        public async Task<ActionResult<Company>> CreateCompany(string caption)
        {
            try
            {
                var createdCompany = await _companyService.CreateCompanyAsync(caption);
                return Ok(createdCompany);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Оновлює назву компанії за її ідентифікатором
        /// </summary>
        /// <param name="id">Ідентифікатор компанії</param>
        /// <param name="companyCaption">Оновлена назва компанії</param>
        /// <returns>
        /// Повертає повідомлення "Дані компанії успішно оновленно", якщо дані компанії оновились успішно; у разі відсутності або виникнення помилки повертає BadRequest з повідомленням про помилку
        /// </returns>
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

        /// <summary>
        /// Видаляє компанію за її ідентифікатором
        /// </summary>
        /// <param name="id">Ідентифікатор компанії, яку потрібно видалити.</param>
        /// <returns>
        /// Повертає повідомлення "Компанію успішно видалено", якщо вона видалена; у разі відсутності або виникнення помилки повертає BadRequest з повідомленням про помилку.
        /// </returns>
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
