using Microsoft.EntityFrameworkCore;

namespace Djini_OOP_Lab8.BLL
{
    public interface IWorkerService
    {
        Task<Worker> GetWorkerAsync(int id);
        Task<Worker> CreateWorkerAsync(string name, string adress, string phone, string email);
        Task<string> UpdateWorkerAsync(int id, string name, string adress, string phone, string email);
        Task<string> DeleteWorkerAsync(int id);
    }
    public class WorkerService : IWorkerService
    {
        private readonly ApplicationContext db;
        public WorkerService(ApplicationContext context)
        {
            db = context;
        }
        public async Task<Worker> GetWorkerAsync(int id)
        {
            var existingWorker = await db.Workers.FirstOrDefaultAsync(w => w.Id == id);
            if (existingWorker != null)
            {
                return existingWorker;
            }
            throw new InvalidOperationException("Працівника з таким id не існує");
        }

        public async Task<Worker> CreateWorkerAsync(string name, string adress, string phone, string email)
        {
            Worker worker = new Worker() { Name = name, Adress = adress, Phone = phone, Email = email };
            db.Workers.Add(worker);
            await db.SaveChangesAsync();
            return worker;
        }

        public async Task<string> UpdateWorkerAsync(int id, string name, string adress, string phone, string email)
        {
            var existingWorker = await db.Workers.FirstOrDefaultAsync(w => w.Id == id);
            if (existingWorker != null)
            {
                db.Entry(existingWorker).CurrentValues.SetValues(new Worker() { Id = id, Name = name, Adress = adress, Phone=phone, Email = email});
                await db.SaveChangesAsync();
                return "Дані працівника успішно оновленно";
            }
            throw new InvalidOperationException("Працівника з таким id не знайдено");
        }

        public async Task<string> DeleteWorkerAsync(int id)
        {

            var existingWorker = await db.Workers.FindAsync(id);
            if (existingWorker != null)
            {
                db.Workers.Remove(existingWorker);
                await db.SaveChangesAsync();
                if (db.Database.IsSqlServer())
                {
                    var maxId = db.Workers.Max(w => (int?)w.Id) ?? 0;
                    db.Database.ExecuteSql($"DBCC CHECKIDENT ('Workers', RESEED, {maxId})");
                }
                return "Працівника успішно видалено";
            }
            throw new InvalidOperationException("Працівника з таким id не знайдено");
        }
    }
}
