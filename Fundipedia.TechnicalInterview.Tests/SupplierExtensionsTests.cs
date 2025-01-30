using Fundipedia.TechnicalInterview.Model.Extensions;
using Fundipedia.TechnicalInterview.Model.Supplier;

namespace Fundipedia.TechnicalInterview.Tests
{
    public class SupplierExtensionsTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void IsActive_Returns_True_WhenActivationDateIsToday()
        {
            // Arrange
            var supplier = new Supplier
            {
                ActivationDate = DateTime.Today
            };

            // Act
            var result = supplier.IsActive();

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void IsActive_Returns_True_WhenActivationDateIsInThePast()
        {
            // Arrange
            var supplier = new Supplier
            {
                ActivationDate = DateTime.Today.AddDays(-1)  // Yesterday
            };

            // Act
            var result = supplier.IsActive();

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void IsActive_Returns_False_WhenActivationDateIsInTheFuture()
        {
            // Arrange
            var supplier = new Supplier
            {
                ActivationDate = DateTime.Today.AddDays(1)  // Tomorrow
            };

            // Act
            var result = supplier.IsActive();

            // Assert
            Assert.IsFalse(result);
        }
    }
}