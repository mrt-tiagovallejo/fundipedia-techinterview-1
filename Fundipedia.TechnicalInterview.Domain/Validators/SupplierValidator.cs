using Fundipedia.TechnicalInterview.Model.Supplier;
using System;

namespace Fundipedia.TechnicalInterview.Domain.Validators
{
    public class SupplierValidator : ISupplierValidator
    {
        private readonly IEmailValidator _emailValidator;
        private readonly IPhoneValidator _phoneValidator;

        public SupplierValidator(IEmailValidator emailValidator, IPhoneValidator phoneValidator)
        {
            _emailValidator = emailValidator;
            _phoneValidator = phoneValidator;
        }

        public ValidationResult Validate(Supplier supplier)
        {
            var result = new ValidationResult();

            // Validate activation date
            if (supplier.ActivationDate <= DateTime.Today)
            {
                result.AddError("Activation date must be tomorrow or later.");
            }

            // Validate email format if existent
            // (assuming email is not mandatory)
            foreach (var email in supplier.Emails)
            {
                if (!_emailValidator.IsValid(email.EmailAddress))
                {
                    result.AddError($"Invalid email format: {email.EmailAddress}");
                }
            }

            // Validate phone number if existent
            // (assuming phone number is not mandatory)
            foreach (var phone in supplier.Phones)
            {
                if (!_phoneValidator.IsValid(phone.PhoneNumber))
                {
                    result.AddError($"Invalid phone number: {phone.PhoneNumber}");
                }
            }

            return result;
        }
    }
}
