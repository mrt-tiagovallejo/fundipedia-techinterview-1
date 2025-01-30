using Fundipedia.TechnicalInterview.Controllers;
using Fundipedia.TechnicalInterview.Domain;
using Fundipedia.TechnicalInterview.Domain.Services;
using Fundipedia.TechnicalInterview.Model.Supplier;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Fundipedia.TechnicalInterview.Tests
{
    public class SuppliersControllerTests
    {
        private Mock<ISupplierService> _mockSupplierService;
        private SuppliersController _controller;

        [SetUp]
        public void Setup()
        {
            // Mock ISupplierService
            _mockSupplierService = new Mock<ISupplierService>();

            // Initialize the controller with the mocked service
            _controller = new SuppliersController(_mockSupplierService.Object);
        }

        [Test]
        public async Task GetSupplier_Returns_Suppliers()
        {
            // Arrange
            var suppliers = new List<Supplier>
            {
                new Supplier { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe" },
                new Supplier { Id = Guid.NewGuid(), FirstName = "Jane", LastName = "Doe" }
            };

            _mockSupplierService.Setup(service => service.GetSuppliers()).ReturnsAsync(suppliers);

            // Act
            var result = await _controller.GetSupplier();

            // Assert
            Assert.That(result.Value, Is.EqualTo(suppliers));
        }

        [Test]
        public async Task GetSupplier_Returns_Supplier_WhenSupplierExists()
        {
            // Arrange
            var supplierId = Guid.NewGuid();

            var supplier = new Supplier
            {
                Id = supplierId, 
                FirstName = "John", 
                LastName = "Doe"
            };

            _mockSupplierService.Setup(service => service.GetSupplier(supplierId)).ReturnsAsync(supplier);

            // Act
            var result = await _controller.GetSupplier(supplierId);

            // Assert
            Assert.That(result.Value, Is.EqualTo(supplier));
        }

        [Test]
        public async Task GetSupplier_Returns_NotFound_WhenSupplierDoesNotExist()
        {
            // Arrange
            var supplierId = Guid.NewGuid();
            _mockSupplierService.Setup(service => service.GetSupplier(supplierId)).ReturnsAsync((Supplier)null);

            // Act
            var result = await _controller.GetSupplier(supplierId);

            // Assert
            var notFoundResult = result.Result as NotFoundResult;
            Assert.That(notFoundResult, Is.Not.Null);
            Assert.That(notFoundResult.StatusCode, Is.EqualTo(404));
        }

        [Test]
        public async Task PostSupplier_Returns_CreatedAtAction_WhenValidSupplier()
        {
            // Arrange
            var supplier = new Supplier
            {
                Id = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe"
            };

            var validationResult = new ValidationResult();
            _mockSupplierService.Setup(service => service.InsertSupplier(It.IsAny<Supplier>())).ReturnsAsync(validationResult);

            // Act
            var result = await _controller.PostSupplier(supplier);

            // Assert
            var createdAtActionResult = result.Result as CreatedAtActionResult;
            Assert.That(createdAtActionResult, Is.Not.Null);
            Assert.That(createdAtActionResult.StatusCode, Is.EqualTo(201));
            Assert.That(createdAtActionResult.ActionName, Is.EqualTo("GetSupplier"));
            Assert.That(createdAtActionResult.Value, Is.EqualTo(supplier));
        }

        [Test]
        public async Task PostSupplier_Returns_BadRequest_WhenInvalidSupplier()
        {
            // Arrange
            var supplier = new Supplier
            {
                Id = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe"
            };

            var validationResult = new ValidationResult();
            validationResult.AddError("dummy error to make it invalid");
            _mockSupplierService.Setup(service => service.InsertSupplier(It.IsAny<Supplier>())).ReturnsAsync(validationResult);

            // Act
            var result = await _controller.PostSupplier(supplier);

            // Assert
            var badRequestResult = result.Result as BadRequestObjectResult;
            Assert.That(badRequestResult, Is.Not.Null);
            Assert.That(badRequestResult.StatusCode, Is.EqualTo(400));
        }

        [Test]
        public async Task DeleteSupplier_Returns_Supplier_WhenDeletedSuccessfully()
        {
            // Arrange
            var supplierId = Guid.NewGuid();
            var supplier = new Supplier
            {
                Id = supplierId,
                FirstName = "John",
                LastName = "Doe"
            };
            _mockSupplierService.Setup(service => service.DeleteSupplier(supplierId)).ReturnsAsync(supplier);

            // Act
            var result = await _controller.DeleteSupplier(supplierId);

            // Assert
            Assert.That(result.Value, Is.EqualTo(supplier));
        }

        [Test]
        public async Task DeleteSupplier_Returns_Null_WhenSupplierDoesNotExist()
        {
            // Arrange
            var supplierId = Guid.NewGuid();
            _mockSupplierService.Setup(service => service.DeleteSupplier(supplierId)).ReturnsAsync((Supplier)null);

            // Act
            var result = await _controller.DeleteSupplier(supplierId);

            // Assert
            Assert.That(result.Value, Is.Null);
        }

        [Test]
        public async Task DeleteSupplier_Returns_InternalServerError_WhenExceptionThrown()
        {
            // Arrange
            var supplierId = Guid.NewGuid();
            var exceptionMessage = $"Supplier {supplierId} is active, can't be deleted";

            // Simulates active supplier exception
            // TODO: supplier service should return validation error instead of exception
            _mockSupplierService.Setup(service => service.DeleteSupplier(supplierId))
                                .ThrowsAsync(new Exception(exceptionMessage));

            // Act & Assert
            var ex = Assert.ThrowsAsync<Exception>(async () => await _controller.DeleteSupplier(supplierId));
            Assert.That(ex.Message, Is.EqualTo(exceptionMessage));
        }
    }
}
