using Djini_OOP_Lab8.BLL;
using Microsoft.AspNetCore.Mvc;

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

        /// <summary>
        /// Отримує вакансію за її ідентифікатором
        /// </summary>
        /// <param name="id">Ідентифікатор вакансії</param>
        /// <returns>
        /// Повертає об'єкт vacancy, якщо знайдено відповідний запис; у разі відсутності або виникнення помилки повертає BadRequest з повідомленням про помилку.
        /// </returns>
        [ProducesResponseType(typeof(Vacancy), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

        /// <summary>
        /// Створює нову вакансію
        /// </summary>
        /// <param name="caption">Назва вакансії</param>
        /// <param name="description">Опис вакансії</param>
        /// <param name="companyId">Ідентифікатор компанії, що надає вакансію</param>
        /// <returns>
        /// Повертає рядок, що містить створений об'єкт vacancy, якщо знайдено відповідний запис; у разі відсутності або виникнення помилки повертає BadRequest з повідомленням про помилку
        /// </returns>
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("{caption}/{description}/{companyId}")]
        public async Task<ActionResult<Vacancy>> CreateVacancy(string caption, string description, int companyId)
        {
            try
            {
                var createdVacancy = await _vacancyService.CreateVacancyAsync(caption, description, companyId);
                return Ok(createdVacancy); ;
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Оновлює дані вакансії за її ідентифікатором
        /// </summary>
        /// <param name="id">Ідентифікатор вакансії</param>
        /// <param name="caption">Оновлена назва вакансії</param>
        /// <param name="description">Оновлений опис вакансії</param>
        /// <param name="companyId">Оновлений ідентифікатор компанії, що надає вакансію</param>
        /// <returns>
        /// Повертає повідомлення "Дані вакансії успішно оновленно", якщо вакансія оновилась успішно; у разі відсутності або виникнення помилки повертає BadRequest з повідомленням про помилку
        /// </returns>
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

        /// <summary>
        /// Видаляє вакансію за її ідентифікатором
        /// </summary>
        /// <param name="id">Ідентифікатор вакансії, яку потрібно видалити</param>
        /// <returns>
        /// Повертає повідомлення "Вакансію успішно видалено", якщо вакансію було видалено; у разі відсутності або виникнення помилки повертає BadRequest з повідомленням про помилку.
        /// </returns>
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
