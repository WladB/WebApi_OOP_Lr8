using Djini_OOP_Lab8.BLL;
using Microsoft.AspNetCore.Mvc;

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

        /// <summary>
        /// Отримує обліковий запис працівника за його ідентифікатором
        /// </summary>
        /// <param name="id">Ідентифікатор працівника</param>
        /// <returns>
        /// Повертає рядок, що містить створений об'єкт worker, якщо знайдено відповідний запис; у разі відсутності або виникнення помилки повертає BadRequest з повідомленням про помилку
        /// </returns>
        [ProducesResponseType(typeof(Worker), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

        /// <summary>
        /// Створює обліковий запис нового працівника
        /// </summary>
        /// <param name="workerName">Ім'я працівника</param>
        /// <param name="workerAdress">Адреса працівника</param>
        /// <param name="workerPhone">Телефон працівника</param>
        /// <param name="workerEmail">Ел. адреса працівника</param>
        /// <returns>
        /// Повертає рядок, що містить створений об'єкт worker, якщо знайдено відповідний запис; у разі відсутності або виникнення помилки повертає BadRequest з повідомленням про помилку
        /// </returns>
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("{workerName}/{workerAdress}/{workerPhone}/{workerEmail}")]
        public async Task<ActionResult<Worker>> CreateWorker(string workerName, string workerAdress, string workerPhone, string workerEmail)
        {
            try
            {
                var createdWorker = await _workerService.CreateWorkerAsync(workerName, workerAdress, workerPhone, workerEmail);
                return Ok(createdWorker);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Оновлює дані в обліковому записі працівника за його ідентифікатором
        /// </summary>
        /// <param name="id">Ідентифікатор працівника</param>
        /// <param name="workerName">Оновлене ім'я працівника</param>
        /// <param name="workerAdress">Оновлена адреса працівника</param>
        /// <param name="workerPhone">Оновлений телефон працівника</param>
        /// <param name="workerEmail">Оновлена Ел. адреса працівника</param>
        /// <returns>
        /// Повертає повідомлення "Дані працівника успішно оновленно", якщо дані працівника оновились успішно; у разі відсутності або виникнення помилки повертає BadRequest з повідомленням про помилку
        /// </returns>
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

        /// <summary>
        /// Видаляє обліковий запис працівника за його ідентифікатором
        /// </summary>
        /// <param name="id">Ідентифікатор облікового запису працівника, який потрібно видалити</param>
        /// <returns>
        /// Повертає повідомлення "Дані працівника успішно видалено", якщо дані працівника було видалено; у разі відсутності або виникнення помилки повертає BadRequest з повідомленням про помилку.
        /// </returns>
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
