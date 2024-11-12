using Djini_OOP_Lab8.BLL;
using Microsoft.AspNetCore.Mvc;

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

        /// <summary>
        /// Отримує резюме за ідентифікаторами вакансії та працівника
        /// </summary>
        /// <param name="vacancyId">Ідентифікатор вакансії</param>
        /// <param name="workerId">Ідентифікатор працівника</param>
        /// <returns>
        /// Повертає об'єкт resume, якщо знайдено відповідний запис; у разі відсутності або виникнення помилки повертає BadRequest з повідомленням про помилку.
        /// </returns>
        [ProducesResponseType(typeof(Resume), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

        /// <summary>
        /// Створює нове резюме
        /// </summary>
        /// <param name="vacancyId">Ідентифікатор вакансії</param>
        /// <param name="workerId">Ідентифікатор працівника</param>
        /// <param name="resumeContent">Вміст резюме</param>
        /// <returns>
        /// Повертає рядок, що містить створений об'єкт resume, якщо знайдено відповідний запис; у разі відсутності або виникнення помилки повертає BadRequest з повідомленням про помилку
        /// </returns>
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

        /// <summary>
        /// Оновлює дані резюме за ідентифікаторами вакансії та працівника
        /// </summary>
        /// <param name="vacancyId">Ідентифікатор вакансії</param>
        /// <param name="workerId">Ідентифікатор працівника</param>
        /// <param name="resumeContent">Оновлений вміст резюме</param>
        /// <returns>
        /// Повертає повідомлення "Дані резюме успішно оновленно", якщо резюме оновилась успішно; у разі відсутності або виникнення помилки повертає BadRequest з повідомленням про помилку
        /// </returns>
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

        /// <summary>
        /// Видаляє резюме за ідентифікаторами вакансії та працівника
        /// </summary>
        /// <param name="vacancyId">Ідентифікатор вакансії</param>
        /// <param name="workerId">Ідентифікатор працівника</param>
        /// <returns>
        /// Повертає повідомлення "Резюме успішно видалено", якщо резюме було видалено; у разі відсутності або виникнення помилки повертає BadRequest з повідомленням про помилку.
        /// </returns>
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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