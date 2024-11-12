using Djini_OOP_Lab8.BLL;
using Microsoft.AspNetCore.Mvc;
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

        /// <summary>
        /// Отримує відгук за ідентифікаторами компанії та працівника
        /// </summary>
        /// <param name="companyId">Ідентифікатор команії</param>
        /// <param name="workerId">Ідентифікатор працівника</param>
        /// <returns>
        /// Повертає об'єкт feedback, якщо знайдено відповідний запис; у разі відсутності або виникнення помилки повертає BadRequest з повідомленням про помилку.
        /// </returns>
        [ProducesResponseType(typeof(Feedback), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

        /// <summary>
        /// Створює новий відгук
        /// </summary>
        /// <param name="companyId">Ідентифікатор команії</param>
        /// <param name="workerId">Ідентифікатор працівника</param>
        /// <param name="resumeContent">Вміст відгуку</param>
        /// <returns>
        /// Повертає рядок, що містить створений об'єкт feedback, якщо знайдено відповідний запис; у разі відсутності або виникнення помилки повертає BadRequest з повідомленням про помилку
        /// </returns
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

        /// <summary>
        /// Оновлює дані відгуку за ідентифікаторами вакансії та працівника
        /// </summary>
        /// <param name="companyId">Ідентифікатор команії</param>
        /// <param name="workerId">Ідентифікатор працівника</param>
        /// <param name="resumeContent">Оновлений вміст відгуку</param>
        /// <returns>
        /// Повертає повідомлення "Дані відгуку успішно оновленно", якщо відгук оновилась успішно; у разі відсутності або виникнення помилки повертає BadRequest з повідомленням про помилку
        /// </returns>
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

        /// <summary>
        /// Видаляє відгук за ідентифікаторами вакансії та працівника
        /// </summary>
        /// <param name="companyId">Ідентифікатор команії</param>
        /// <param name="workerId">Ідентифікатор працівника</param>
        /// <returns>
        /// Повертає повідомлення "Відгук успішно видалено", якщо відгуку було видалено; у разі відсутності або виникнення помилки повертає BadRequest з повідомленням про помилку.
        /// </returns>
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

