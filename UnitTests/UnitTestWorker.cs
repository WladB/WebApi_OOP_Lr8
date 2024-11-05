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
    public class WorkerControllerTest
    {
        private ApplicationContext _context;
        private WorkerController _controller;
        private WorkerService _service;

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
            _service = new WorkerService(_context);
            _controller = new WorkerController(_service);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [TestMethod]
        public async Task GetWorker_ReturnsWorker()
        {
            // Arrange
            var worker = new Worker
            {
                Name = "Ivan",
                Adress = "123 Khreshchatyk Street, Apt. 45, Kyiv, 01001, Ukraine",
                Phone = "+380111223344",
                Email = "testemail@ukr.net"
            };


            // Act
            _context.Workers.Add(worker);
            await _context.SaveChangesAsync();
            var result = await _controller.GetWorker(1);

            // Assert
            Assert.IsNotNull(result.Value);
            Assert.AreEqual(worker.Id, result.Value.Id);
            Assert.AreEqual(worker.Name, result.Value.Name);
            Assert.AreEqual(worker.Adress, result.Value.Adress);
            Assert.AreEqual(worker.Phone, result.Value.Phone);
            Assert.AreEqual(worker.Email, result.Value.Email);


        }
        [TestMethod]
        public async Task CreateWorker_ReturnsWorker()
        {
           
            // Act
            await _controller.CreateWorker("Ivan", "123 Khreshchatyk Street Apt. 45 Kyiv 01001 Ukraine",
              "+380111223344", "testemail@ukr.net");
            var result = await _context.Workers.FirstOrDefaultAsync(w => w.Id == 1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("Ivan", result.Name);
            Assert.AreEqual("123 Khreshchatyk Street Apt. 45 Kyiv 01001 Ukraine", result.Adress);
            Assert.AreEqual("+380111223344", result.Phone);
            Assert.AreEqual("testemail@ukr.net", result.Email);

        }

        [TestMethod]
        public async Task UpdateWorker_ReturnsOk()
        {
            // Arrange
            var worker = new Worker
            {
                Name = "Ivan",
                Adress = "123 Khreshchatyk Street, Apt. 45, Kyiv, 01001, Ukraine",
                Phone = "+380111223344",
                Email = "testemail@ukr.net",
            };

            // Act
            _context.Workers.Add(worker);
            await _context.SaveChangesAsync();

            await _controller.UpdateWorker(1, "Andry", "Updated Adress", "+380444556677", "testemail@gmail.com");
            var result = await _context.Workers.FirstOrDefaultAsync(w => w.Id == 1);

            // Assert
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("Andry", result.Name);
            Assert.AreEqual("Updated Adress", result.Adress);
            Assert.AreEqual("+380444556677", result.Phone);
            Assert.AreEqual("testemail@gmail.com", result.Email);

        }

        [TestMethod]
        public async Task DeleteWorker_ReturnsOk()
        {
            // Arrange
            var worker = new Worker
            {
                Name = "Ivan",
                Adress = "123 Khreshchatyk Street, Apt. 45, Kyiv, 01001, Ukraine",
                Phone = "+380111223344",
                Email = "testemail@ukr.net"
            };

            // Act
            _context.Workers.Add(worker);
            await _context.SaveChangesAsync();

            var result = _controller.DeleteWorker(1).Result;
            var resultAfterDeleting = _controller.DeleteWorker(1).Result;

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.IsInstanceOfType(resultAfterDeleting, typeof(BadRequestObjectResult));
        }
    }
}
