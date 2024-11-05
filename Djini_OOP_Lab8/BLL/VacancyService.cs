using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;
using System;

namespace Djini_OOP_Lab8.BLL
{
    public interface IVacancyService
    {
        Task<Vacancy> GetVacancyAsync(int id);
        Task<Vacancy> CreateVacancyAsync(string caption, string description, int companyId);
        Task<string> UpdateVacancyAsync(int id, string caption, string description, int companyId);
        Task<string> DeleteVacancyAsync(int id);
    }
    public class VacancyService : IVacancyService
    {
        private readonly ApplicationContext db;
        public VacancyService(ApplicationContext context)
        {
            db = context;
        }
        public async Task<Vacancy> GetVacancyAsync(int id)
        {
            var existingVacancy = await db.Vacancies.FirstOrDefaultAsync(v => v.Id == id);
            if (existingVacancy != null)
            {
                return existingVacancy;
            }
            throw new InvalidOperationException("Вакансії з таким id не існує");
        }

        public async Task<Vacancy> CreateVacancyAsync(string caption, string description, int companyId)
        {
            if (await db.Companies.AnyAsync(c => c.Id == companyId))
            {
                Vacancy vacancy = new Vacancy() { Caption = caption, Description = description, CompanyId = companyId };
                db.Vacancies.Add(vacancy);
                await db.SaveChangesAsync();
                return vacancy;
            }
            throw new InvalidOperationException("Невдалось створити вакансію, компанії з таким id не існує");

        }

        public async Task<string> UpdateVacancyAsync(int id, string caption, string description, int companyId)
        {
            var existingVacancy = await db.Vacancies.FirstOrDefaultAsync(v => v.Id == id);
            if (existingVacancy != null)
            {
                if (await db.Companies.AnyAsync(c => c.Id == companyId))
                {
                    db.Entry(existingVacancy).CurrentValues.SetValues(new Vacancy() { Id = id, Caption = caption, Description = description, CompanyId = companyId });
                    await db.SaveChangesAsync();
                    return "Дані вакансії успішно оновленно";
                }
                throw new InvalidOperationException("Компанії з таким id не знайдено");
            }
            throw new InvalidOperationException("Вакансії з таким id не знайдено");
        }

        public async Task<string> DeleteVacancyAsync(int id)
        {

            var existingVacancy = await db.Vacancies.FindAsync(id);
            if (existingVacancy != null)
            {
                db.Vacancies.Remove(existingVacancy);
                await db.SaveChangesAsync();
                if (db.Database.IsSqlServer())
                {
                    var maxId = db.Vacancies.Max(v => (int?)v.Id) ?? 0;
                    db.Database.ExecuteSql($"DBCC CHECKIDENT ('Vacancies', RESEED, {maxId})");
                }
                return "Вакансію успішно видалено";
            }
            throw new InvalidOperationException("Вакансію з таким id не знайдено");
        }
    }
}
