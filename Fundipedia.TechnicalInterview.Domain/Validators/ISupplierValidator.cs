using Fundipedia.TechnicalInterview.Model.Supplier;

namespace Fundipedia.TechnicalInterview.Domain.Validators
{
    public interface ISupplierValidator
    {
        ValidationResult Validate(Supplier supplier);
    }
}
