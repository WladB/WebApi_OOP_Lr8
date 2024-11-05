using Microsoft.EntityFrameworkCore;
using Djini_OOP_Lab8;
using Djini_OOP_Lab8.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Djini_OOP_Lab8.BLL;
using System.Numerics;

namespace UnitTests
{
    [TestClass]
    public class CompanyControllerTest
    {
        private ApplicationContext _context;
        private CompanyController _controller;
        private CompanyService _service;

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
            _service = new CompanyService(_context);
            _controller = new CompanyController(_service);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
        
        [TestMethod]
        public async Task GetCompany_ReturnsCompany()
        {
            // Arrange
            var company = new Company { Caption = "Test Company" };


            // Act
            _context.Companies.Add(company);
            await _context.SaveChangesAsync();

            var result = await _controller.GetCompany(1);

            // Assert
            Assert.IsNotNull(result.Value);
            Assert.AreEqual(company.Id, result.Value.Id);
            Assert.AreEqual(company.Caption, result.Value.Caption);
        }

        [TestMethod]
        public async Task CreateCompany_ReturnsCompany()
        {
            // Arrange
            string caption = "Test Company";

            // Act
            await _controller.CreateCompany(caption);
            var result = await _context.Companies.FirstOrDefaultAsync(c => c.Id == 1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("Test Company", result.Caption);
        }

        [TestMethod]
        public async Task UpdateCompany_ReturnsOk()
        {
            // Arrange
            var company = new Company { Caption = "Test Company" };

            // Act
            _context.Companies.Add(company);
            await _context.SaveChangesAsync();

            await _controller.UpdateCompany(1, "Test Company Updated");
            var result = await _context.Companies.FirstOrDefaultAsync(c => c.Id == 1);

            // Assert
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("Test Company Updated", result.Caption);
        }

        [TestMethod]
        public async Task DeleteCompany_ReturnsOk()
        {
            // Arrange
            var company = new Company { Caption = "Test Company" };

            // Act
            _context.Companies.Add(company);
            await _context.SaveChangesAsync();

            var result = _controller.DeleteCompany(1).Result;
            var resultAfterDeleting = _controller.DeleteCompany(1).Result;

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.IsInstanceOfType(resultAfterDeleting, typeof(BadRequestObjectResult));
        }
    }
}
