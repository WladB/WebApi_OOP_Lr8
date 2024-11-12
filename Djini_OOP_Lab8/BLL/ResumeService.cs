using Microsoft.EntityFrameworkCore;

namespace Djini_OOP_Lab8.BLL
{
    public interface IResumeService
    {
        Task<Resume> GetResumeAsync(int vacancyId, int workerId);
        Task<Resume> CreateResumeAsync(int vacancyId, int workerId, string resumeContent);
        Task<string> UpdateResumeAsync(int vacancyId, int workerId, string resumeContent);
        Task<string> DeleteResumeAsync(int vacancyId, int workerId);
    }
    public class ResumeService : IResumeService
    {
        private readonly ApplicationContext db;
        public ResumeService(ApplicationContext context)
        {
            db = context;
        }
        public async Task<Resume> GetResumeAsync(int vacancyId, int workerId)
        {
            var existingResume = await db.Resumes.FirstOrDefaultAsync(r => r.VacancyId == vacancyId && r.WorkerId == workerId) ;
            if (existingResume != null)
            {
                return existingResume;
            }
            throw new InvalidOperationException("Резюме не знайдено");
        }
        public async Task<Resume> CreateResumeAsync(int vacancyId, int workerId, string resumeContent)
        {
            if (!await db.Resumes.AnyAsync(r => r.VacancyId == vacancyId && r.WorkerId == workerId))
            {
                if (await db.Vacancies.AnyAsync(v => v.Id == vacancyId))
                {
                    if (await db.Workers.AnyAsync(w => w.Id == workerId))
                    {
                        Resume resume = new Resume() { VacancyId = vacancyId, WorkerId = workerId, ResumeContent = resumeContent };
                        db.Resumes.Add(resume);
                        await db.SaveChangesAsync();
                        return resume;
                    }
                    throw new InvalidOperationException("Неможливо додати резюме,  такого працівника не існує");
                }
                throw new InvalidOperationException("Неможливо додати резюме, такої вакансії не існує");
            }
            throw new InvalidOperationException("Таке резюме вже існує");

        }

        public async Task<string> UpdateResumeAsync(int vacancyId, int workerId, string resumeContent)
        {
            var existingResume = await db.Resumes.FirstOrDefaultAsync(r => r.VacancyId == vacancyId && r.WorkerId == workerId);
            if (existingResume!=null)
            {
                if (await db.Vacancies.AnyAsync(v => v.Id == vacancyId))
                {
                    if (await db.Workers.AnyAsync(w => w.Id == workerId))
                    {
                        db.Entry(existingResume).CurrentValues.SetValues(new Resume() { VacancyId = vacancyId, WorkerId = workerId, ResumeContent = resumeContent });
                        await db.SaveChangesAsync();
                        return "Дані резюме успішно оновлено";
                    }
                    throw new InvalidOperationException("Неможливо оновити резюме, такого працівника не існує");
                }
                throw new InvalidOperationException("Неможливо оновити резюме, такої вакансії не існує");
            }
            throw new InvalidOperationException("Такого резюме не існує");

        }

        public async Task<string> DeleteResumeAsync(int vacancyId, int workerId)
        {
            var existingResume = await db.Resumes.FirstOrDefaultAsync(r => r.VacancyId == vacancyId && r.WorkerId == workerId);
            if (existingResume != null)
            {
                db.Resumes.Remove(existingResume);
                await db.SaveChangesAsync();
                return "Резюме успішно видалено";
            }
            throw new InvalidOperationException("Резюме не знайдено");
        }
    }
}
