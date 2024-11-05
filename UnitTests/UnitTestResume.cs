using Djini_OOP_Lab8.BLL;
using Djini_OOP_Lab8.Controllers;
using Djini_OOP_Lab8;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace UnitTests
{
    [TestClass]
    public class ResumeControllerTest
    {
        private ApplicationContext _context;
        private ResumeController _controller;
        private ResumeService _service;

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
            _service = new ResumeService(_context);
            _controller = new ResumeController(_service);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [TestMethod]
        public async Task GetResume_ReturnsResume()
        {
            // Arrange
            var vacancy = new Vacancy
            {
                Caption = "C++ Programmer",
                Description = "Шукаємо на роботу відповідального працівника зі знанням C++",
                CompanyId = 1
            };
            var worker = new Worker
            {
                Name = "Ivan",
                Adress = "123 Khreshchatyk Street, Apt. 45, Kyiv, 01001, Ukraine",
                Phone = "+380111223344",
                Email = "testemail@ukr.net"
            };
            var resume = new Resume
            {
                VacancyId = 1,
                WorkerId = 1,
                ResumeContent = "Test resume content"
            };

            // Act
            _context.Vacancies.Add(vacancy);
            _context.Workers.Add(worker);
            _context.Resumes.Add(resume);
            await _context.SaveChangesAsync();

            var result = await _controller.GetResume(1, 1);

            // Assert
            Assert.IsNotNull(result.Value);
            Assert.AreEqual(1, result.Value.VacancyId);
            Assert.AreEqual(1, result.Value.WorkerId);
            Assert.AreEqual("Test resume content", result.Value.ResumeContent);
        }

        [TestMethod]
        public async Task CreateResume_ReturnsResume()
        {
            // Arrange
            var vacancy = new Vacancy
            {
                Caption = "C++ Programmer",
                Description = "Шукаємо на роботу відповідального працівника зі знанням C++",
                CompanyId = 1
            };
            var worker = new Worker
            {
                Name = "Ivan",
                Adress = "123 Khreshchatyk Street, Apt. 45, Kyiv, 01001, Ukraine",
                Phone = "+380111223344",
                Email = "testemail@ukr.net"
            };

            // Act
            _context.Vacancies.Add(vacancy);
            _context.Workers.Add(worker);
            await _context.SaveChangesAsync();

            await _controller.CreateResume(1, 1, "Test resume content");
            var result = await _context.Resumes.FirstOrDefaultAsync(r => r.VacancyId == 1 && r.WorkerId == 1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.VacancyId);
            Assert.AreEqual(1, result.WorkerId);
            Assert.AreEqual("Test resume content", result.ResumeContent);

        }

        [TestMethod]
        public async Task UpdateResume_ReturnsOk()
        {
            // Arrange
            var vacancy = new Vacancy
            {
                Caption = "C++ Programmer",
                Description = "Шукаємо на роботу відповідального працівника зі знанням C++",
                CompanyId = 1
            };
            var worker = new Worker
            {
                Name = "Ivan",
                Adress = "123 Khreshchatyk Street, Apt. 45, Kyiv, 01001, Ukraine",
                Phone = "+380111223344",
                Email = "testemail@ukr.net"
            };
            var resume = new Resume
            {
                VacancyId = 1,
                WorkerId = 1,
                ResumeContent = "Test resume content"
            };

            // Act
            _context.Vacancies.Add(vacancy);
            _context.Workers.Add(worker);
            _context.Resumes.Add(resume);
            await _context.SaveChangesAsync();

            await _controller.UpdateResume(1, 1, "Test resume content updated");
            var result = await _context.Resumes.FirstOrDefaultAsync(r => r.VacancyId == 1 && r.WorkerId == 1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Test resume content updated", result.ResumeContent);

        }
        [TestMethod]
        public async Task DeleteResume_ReturnsOk()
        {
            var vacancy = new Vacancy
            {
                Caption = "C++ Programmer",
                Description = "Шукаємо на роботу відповідального працівника зі знанням C++",
                CompanyId = 1
            };
            var worker = new Worker
            {
                Name = "Ivan",
                Adress = "123 Khreshchatyk Street, Apt. 45, Kyiv, 01001, Ukraine",
                Phone = "+380111223344",
                Email = "testemail@ukr.net"
            };
            var resume = new Resume
            {
                VacancyId = 1,
                WorkerId = 1,
                ResumeContent = "Test resume content"
            };

            // Act
            _context.Vacancies.Add(vacancy);
            _context.Workers.Add(worker);
            _context.Resumes.Add(resume);
            await _context.SaveChangesAsync();

            var result = _controller.DeleteResume(1, 1).Result;
            var resultAfterDeleting = _controller.DeleteResume(1, 1).Result;

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.IsInstanceOfType(resultAfterDeleting, typeof(BadRequestObjectResult));
        }
    }
}

