using Fundipedia.TechnicalInterview.Domain.Services;
using Fundipedia.TechnicalInterview.Model.Supplier;
using Fundipedia.TechnicalInterview.Domain.Validators;
using Moq;
using Fundipedia.TechnicalInterview.Domain;
using Fundipedia.TechnicalInterview.Data.Repositories;

namespace Fundipedia.TechnicalInterview.Tests
{
    [TestFixture]
    public class SupplierServiceTests
    {
        private Mock<ISupplierRepository> _mockRepository;
        private Mock<ISupplierValidator> _mockValidator;
        private SupplierService _service;

        [SetUp]
        public void Setup()
        {
            _mockRepository = new Mock<ISupplierRepository>();
            _mockValidator = new Mock<ISupplierValidator>();
            _service = new SupplierService(_mockRepository.Object, _mockValidator.Object);
        }

        [Test]
        public async Task GetSupplier_Returns_Supplier_WhenSupplierExists()
        {
            // Arrange
            var supplier = new Supplier
            {
                Id = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                Emails = new List<Email> { new Email { EmailAddress = "john@doe.com" } }
            };
            _mockRepository.Setup(r => r.GetSupplierAsync(supplier.Id)).ReturnsAsync(supplier);

            // Act
            var result = await _service.GetSupplier(supplier.Id);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(supplier));
            Assert.That(result.Emails.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task GetSupplier_Returns_Null_WhenSupplierDoesNotExist()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetSupplierAsync(It.IsAny<Guid>())).ReturnsAsync((Supplier)null);

            // Act
            var result = await _service.GetSupplier(Guid.NewGuid());

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task GetSuppliers_Returns_Suppliers()
        {
            // Arrange
            var suppliers = new List<Supplier>
            {
                new Supplier
                {
                    Id = Guid.NewGuid(),
                    FirstName = "John",
                    LastName = "Doe",
                    Emails = new List<Email> { new Email { EmailAddress = "john@doe.com" } }
                },
                new Supplier
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Jane",
                    LastName = "Doe",
                    Phones = new List<Phone> { new Phone { PhoneNumber = "123456789" } }
                }
            };
            _mockRepository.Setup(r => r.GetAllSuppliersAsync()).ReturnsAsync(suppliers);

            // Act
            var result = await _service.GetSuppliers();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.First().Emails.Count, Is.EqualTo(1));
            Assert.That(result.Skip(1).First().Phones.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task InsertSupplier_Returns_ValidResult_WhenValidSupplierIsInserted()
        {
            // Arrange
            var supplier = new Supplier
            {
                Id = Guid.NewGuid(),
                FirstName = "Jimi",
                LastName = "Hendrix",
                Emails = new List<Email> { new Email { EmailAddress = "jimi@hendrix.com" } }
            };
            var validResult = new ValidationResult();
            _mockValidator.Setup(v => v.Validate(supplier)).Returns(validResult);

            // Act
            var result = await _service.InsertSupplier(supplier);

            // Assert
            Assert.That(result.IsValid, Is.True);
            _mockRepository.Verify(r => r.AddSupplierAsync(supplier), Times.Once);
        }

        [Test]
        public async Task InsertSupplier_Returns_InvalidResult_WhenSupplierIsInvalid()
        {
            // Arrange
            var supplier = new Supplier { Id = Guid.NewGuid(), FirstName = "Invalid", LastName = "Supplier" };
            var invalidResult = new ValidationResult();
            invalidResult.AddError("invalid");
            _mockValidator.Setup(v => v.Validate(supplier)).Returns(invalidResult);

            // Act
            var result = await _service.InsertSupplier(supplier);

            // Assert
            Assert.That(result.IsValid, Is.False);
            _mockRepository.Verify(r => r.AddSupplierAsync(It.IsAny<Supplier>()), Times.Never);
        }

        [Test]
        public async Task DeleteSupplier_Deletes_Supplier_WhenSupplierIsInactive()
        {
            // Arrange
            var supplier = new Supplier
            {
                Id = Guid.NewGuid(),
                ActivationDate = DateTime.Now.AddDays(1)
            };
            _mockRepository.Setup(r => r.GetSupplierAsync(supplier.Id)).ReturnsAsync(supplier);

            // Act
            var deletedSupplier = await _service.DeleteSupplier(supplier.Id);

            // Assert
            Assert.That(deletedSupplier, Is.Not.Null);
            _mockRepository.Verify(r => r.RemoveSupplierAsync(supplier), Times.Once);
        }

        [Test]
        public async Task DeleteSupplier_Throws_Exception_WhenSupplierIsActive()
        {
            // Arrange
            var supplier = new Supplier
            {
                Id = Guid.NewGuid(),
                ActivationDate = DateTime.Now.AddDays(-1)
            };
            _mockRepository.Setup(r => r.GetSupplierAsync(supplier.Id)).ReturnsAsync(supplier);

            // Act & Assert
            var ex = Assert.ThrowsAsync<Exception>(async () => await _service.DeleteSupplier(supplier.Id));
            Assert.That(ex.Message, Does.Contain($"Supplier {supplier.Id} is active, can't be deleted"));
            _mockRepository.Verify(r => r.RemoveSupplierAsync(It.IsAny<Supplier>()), Times.Never);
        }
    }
}