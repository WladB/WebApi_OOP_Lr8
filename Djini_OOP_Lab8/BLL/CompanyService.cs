using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;


namespace Djini_OOP_Lab8.BLL
{
    public interface ICompanyService
    {
        Task<Company> GetCompanyAsync(int id);
        Task<Company> CreateCompanyAsync(string caption);
        Task<string> UpdateCompanyAsync(int id, string caption);
        Task<string> DeleteCompanyAsync(int id);
    }
    public class CompanyService: ICompanyService
    {
        private readonly ApplicationContext db;
        public CompanyService(ApplicationContext context)
        {
            db = context;
        }
        public async Task<Company> GetCompanyAsync(int id)
        {
            var existingCompany = await db.Companies.FirstOrDefaultAsync(c => c.Id == id);
            if (existingCompany != null)
            {
                return existingCompany;
            }
            throw new InvalidOperationException("Компанії з таким id не існує");
        }
        public async Task<Company> CreateCompanyAsync(string caption)
        {
            Company company = new Company() { Caption = caption };
                    db.Companies.Add(company);
                    await db.SaveChangesAsync();
                    return company; 
        }
        public async Task<string> UpdateCompanyAsync(int id, string caption)
        {
            var existingCompany = await db.Companies.FirstOrDefaultAsync(c => c.Id == id);
            if (existingCompany != null)
            {
                db.Entry(existingCompany).CurrentValues.SetValues(new Company() { Id = id, Caption = caption });
                await db.SaveChangesAsync();
                return "Дані компанії успішно оновленно";

            }
            throw new InvalidOperationException("Компанії з таким id не знайдено");
        }

        public async Task<string> DeleteCompanyAsync(int id)
        {
          
                var existingCompany = await db.Companies.FindAsync(id);
            if (existingCompany != null)
            {
                db.Companies.Remove(existingCompany);
                await db.SaveChangesAsync();

                if (db.Database.IsSqlServer())
                {
                    var maxId = db.Companies.Max(c => (int?)c.Id) ?? 0;
                    db.Database.ExecuteSql($"DBCC CHECKIDENT ('Companies', RESEED, {maxId})");
                }
                return "Компанію успішно видалено";
            }
            throw new InvalidOperationException("Компанії з таким id не знайдено");
        }
    }
}
