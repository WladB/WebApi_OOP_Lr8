using Djini_OOP_Lab8.BLL;
using Djini_OOP_Lab8.Controllers;
using Djini_OOP_Lab8;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class FeedbackControllerTest
    {
        private ApplicationContext _context;
        private FeedbackController _controller;
        private FeedbackService _service;

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
            _service = new FeedbackService(_context);
            _controller = new FeedbackController(_service);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [TestMethod]
        public async Task GetFeedback_ReturnsFeedback()
        {
            // Arrange
            var company = new Company { Caption = "Test Company" };
            var worker = new Worker
            {
                Name = "Ivan",
                Adress = "123 Khreshchatyk Street, Apt. 45, Kyiv, 01001, Ukraine",
                Phone = "+380111223344",
                Email = "testemail@ukr.net"
            };
            var feedback = new Feedback
            {
                WorkerId = 1,
                CompanyId = 1,
                FeedbackContent = "Test feedback content"
            };

            // Act
            _context.Workers.Add(worker);
            _context.Companies.Add(company);
            _context.Feedbacks.Add(feedback);
            await _context.SaveChangesAsync();

            var result = await _controller.GetFeedback(1, 1);

            // Assert
            Assert.IsNotNull(result.Value);
            Assert.AreEqual(1, result.Value.WorkerId);
            Assert.AreEqual(1, result.Value.CompanyId);
            Assert.AreEqual(worker, result.Value.Worker);
            Assert.AreEqual(company, result.Value.Company);
            Assert.AreEqual("Test feedback content", result.Value.FeedbackContent);
        }

        [TestMethod]
        public async Task CreateFeedback_ReturnsFeedback()
        {
            // Arrange
            var company = new Company { Caption = "Test Company" };
            var worker = new Worker
            {
                Name = "Ivan",
                Adress = "123 Khreshchatyk Street, Apt. 45, Kyiv, 01001, Ukraine",
                Phone = "+380111223344",
                Email = "testemail@ukr.net"
            };
            
            // Act
            _context.Workers.Add(worker);
            _context.Companies.Add(company);
            await _context.SaveChangesAsync();

            await _controller.CreateFeedback(1, 1, "Test feedback content");
            var result = await _context.Feedbacks.FirstOrDefaultAsync(f => f.WorkerId == 1 && f.CompanyId == 1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.WorkerId);
            Assert.AreEqual(1, result.CompanyId);
            Assert.AreEqual(worker, result.Worker);
            Assert.AreEqual(company, result.Company);
            Assert.AreEqual("Test feedback content", result.FeedbackContent);

        }

        [TestMethod]
        public async Task UpdateFeedback_ReturnsOk()
        {
            // Arrange
            var company = new Company { Caption = "Test Company" };
            var worker = new Worker
            {
                Name = "Ivan",
                Adress = "123 Khreshchatyk Street, Apt. 45, Kyiv, 01001, Ukraine",
                Phone = "+380111223344",
                Email = "testemail@ukr.net"
            };
            var feedback = new Feedback
            {
                WorkerId = 1,
                CompanyId = 1,
                FeedbackContent = "Test feedback content"
            };

            // Act
            _context.Workers.Add(worker);
            _context.Companies.Add(company);
            _context.Feedbacks.Add(feedback);
            await _context.SaveChangesAsync();

            await _controller.UpdateFeedback(1, 1, "Test feedback content updated");
            var result = await _context.Feedbacks.FirstOrDefaultAsync(f => f.WorkerId == 1 && f.CompanyId == 1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Test feedback content updated", result.FeedbackContent);

        }
        [TestMethod]
        public async Task DeleteFeedback_ReturnsOk()
        {
            var company = new Company { Caption = "Test Company" };
            var worker = new Worker
            {
                Name = "Ivan",
                Adress = "123 Khreshchatyk Street, Apt. 45, Kyiv, 01001, Ukraine",
                Phone = "+380111223344",
                Email = "testemail@ukr.net"
            };
            var feedback = new Feedback
            {
                WorkerId = 1,
                CompanyId = 1,
                FeedbackContent = "Test feedback content"
            };

            // Act
            _context.Companies.Add(company);
            _context.Workers.Add(worker);
            _context.Feedbacks.Add(feedback);
            await _context.SaveChangesAsync();

            var result = _controller.DeleteFeedback(1, 1).Result;
            var resultAfterDeleting = _controller.DeleteFeedback(1, 1).Result;

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.IsInstanceOfType(resultAfterDeleting, typeof(BadRequestObjectResult));
        }
    }
}

