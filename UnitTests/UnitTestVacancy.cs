using Djini_OOP_Lab8.BLL;
using Djini_OOP_Lab8.Controllers;
using Djini_OOP_Lab8;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace UnitTests
{
    [TestClass]
    public class VacancyControllerTest
    {
        private ApplicationContext _context;
        private VacanciesController _controller;
        private VacancyService _service;

        [TestInitialize]
        public void Setup()
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .AddEntityFrameworkProxies()
                .BuildServiceProvider();

            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .UseInternalServiceProvider(serviceProvider)
                .UseLazyLoadingProxies()
                .Options;

            _context = new ApplicationContext(options);
            _service = new VacancyService(_context);
            _controller = new VacanciesController(_service);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [TestMethod]
        public async Task GetVacancy_ReturnsVacancy()
        {
            // Arrange
            var company = new Company { Caption = "Test Company" };
            var vacancy = new Vacancy
            {
                Caption = "C++ Programmer",
                Description = "Шукаємо на роботу відповідального працівника зі знанням C++",
                CompanyId = 1
            };

            // Act
            _context.Companies.Add(company);
            _context.Vacancies.Add(vacancy);
            await _context.SaveChangesAsync();

            var result = await _controller.GetVacancy(1);

            // Assert
            Assert.IsNotNull(result.Value);
            Assert.AreEqual(1, result.Value.Id);
            Assert.AreEqual("C++ Programmer", result.Value.Caption);
            Assert.AreEqual("Шукаємо на роботу відповідального працівника зі знанням C++", result.Value.Description);
            Assert.AreEqual(1, result.Value.CompanyId);
            Assert.AreEqual(company, result.Value.Company);
        }

        [TestMethod]
        public async Task CreateVacancy_ReturnsVacancy()
        {
            // Arrange
            var company = new Company { Caption = "Test Company" };

            // Act
            _context.Companies.Add(company);
            await _context.SaveChangesAsync();

            await _controller.CreateVacancy("C++ Programmer", "Шукаємо на роботу відповідального працівника зі знанням C++", 1);
            var result = await _context.Vacancies.FirstOrDefaultAsync(v => v.Id == 1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("C++ Programmer", result.Caption);
            Assert.AreEqual("Шукаємо на роботу відповідального працівника зі знанням C++", result.Description);
            Assert.AreEqual(1, result.CompanyId);
            Assert.AreEqual(company, result.Company);

        }

        [TestMethod]
        public async Task UpdateVacancy_ReturnsOk()
        {
            var company1 = new Company { Caption = "Test Company1" };
            var company2 = new Company { Caption = "Test Company2" };
            var vacancy = new Vacancy
            {
                Caption = "C++ Programmer",
                Description = "Шукаємо на роботу відповідального працівника зі знанням C++",
                CompanyId = 1
            };

            // Act
            _context.Companies.Add(company1);
            _context.Companies.Add(company2);
            _context.Vacancies.Add(vacancy);
            await _context.SaveChangesAsync();

            await _controller.UpdateVacancy(1, "Test caption updated", "Test description updated", 2);
            var result = await _context.Vacancies.FirstOrDefaultAsync(v => v.Id == 1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("Test caption updated", result.Caption);
            Assert.AreEqual("Test description updated", result.Description);
            Assert.AreEqual(2, result.CompanyId);
            Assert.AreEqual(company2, result.Company);

        }

        [TestMethod]
        public async Task DeleteVacancy_ReturnsOk()
        {
            // Arrange
            var company = new Company { Caption = "Test Company" };
            var vacancy = new Vacancy
            {
                Caption = "C++ Programmer",
                Description = "Шукаємо на роботу відповідального працівника зі знанням C++",
                CompanyId = 1
            };

            // Act
            _context.Companies.Add(company);
            _context.Vacancies.Add(vacancy);
            await _context.SaveChangesAsync();

            var result = _controller.DeleteVacancy(1).Result;
            var resultAfterDeleting = _controller.DeleteVacancy(1).Result;

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.IsInstanceOfType(resultAfterDeleting, typeof(BadRequestObjectResult));
        }
    }
}
