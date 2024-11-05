using Microsoft.EntityFrameworkCore;

namespace Djini_OOP_Lab8.BLL
{
    public interface IFeedbackService
    {
        Task<Feedback> GetFeedbackAsync(int companyId, int workerId);
        Task<Feedback> CreateFeedbackAsync(int companyId, int workerId, string resumeContent);
        Task<string> UpdateFeedbackAsync(int companyId, int workerId, string resumeContent);
        Task<string> DeleteFeedbackAsync(int companyId, int workerId);
    }
    public class FeedbackService : IFeedbackService
    {
        private readonly ApplicationContext db;
        public FeedbackService(ApplicationContext context)
        {
            db = context;
        }
        public async Task<Feedback> GetFeedbackAsync(int companyId, int workerId)
        {
            var existingFeedback = await db.Feedbacks.FirstOrDefaultAsync(f => f.CompanyId == companyId && f.WorkerId == workerId);
            if (existingFeedback != null)
            {
                return existingFeedback;
            }
            throw new InvalidOperationException("Відгук не знайдено");
        }
        public async Task<Feedback> CreateFeedbackAsync(int companyId, int workerId, string resumeContent)
        {
            if (!await db.Feedbacks.AnyAsync(f => f.CompanyId == companyId && f.WorkerId == workerId))
            {
                if (await db.Companies.AnyAsync(c => c.Id == companyId))
                {
                    if (await db.Workers.AnyAsync(w => w.Id == workerId))
                    {
                        Feedback resume = new Feedback() { CompanyId = companyId, WorkerId = workerId, FeedbackContent = resumeContent };
                        db.Feedbacks.Add(resume);
                        await db.SaveChangesAsync();
                        return resume;
                    }
                    throw new InvalidOperationException("Неможливо додати резюме, такого працівника не існує");
                }
                throw new InvalidOperationException("Неможливо додати відгук, такої компанії не існує");
            }
            throw new InvalidOperationException("Такий відгук вже є");

        }

        public async Task<string> UpdateFeedbackAsync(int companyId, int workerId, string resumeContent)
        {
            var existingFeedback = await db.Feedbacks.FirstOrDefaultAsync(f => f.CompanyId == companyId && f.WorkerId == workerId);
            if (existingFeedback != null)
            {
                if (await db.Companies.AnyAsync(c => c.Id == companyId))
                {
                    if (await db.Workers.AnyAsync(w => w.Id == workerId))
                    {
                        db.Entry(existingFeedback).CurrentValues.SetValues(new Feedback() { CompanyId = companyId, WorkerId = workerId, FeedbackContent = resumeContent });
                        await db.SaveChangesAsync();
                        return "Дані резюме успішно оновлено";
                    }
                    throw new InvalidOperationException("Неможливо оновити резюме, такого працівника не існує");
                }
                throw new InvalidOperationException("Неможливо оновити дані вігуку, такої компанії не існує");
            }
            throw new InvalidOperationException("Такого відгуку не існує");

        }

        public async Task<string> DeleteFeedbackAsync(int companyId, int workerId)
        {
            var existingFeedback = await db.Feedbacks.FirstOrDefaultAsync(f => f.CompanyId == companyId && f.WorkerId == workerId);
            if (existingFeedback != null)
            {
                db.Feedbacks.Remove(existingFeedback);
                await db.SaveChangesAsync();
                return "Відгук успішно видалено";
            }
            throw new InvalidOperationException("Відгук не знайдено");
        }
    }
}

