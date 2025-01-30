using Fundipedia.TechnicalInterview.Model.Supplier;
using System;
using System.Collections.Generic;

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

            ValidateActivationDate(supplier.ActivationDate, result);
            ValidateEmails(supplier.Emails, result);
            ValidatePhones(supplier.Phones, result);

            return result;
        }

        /// <summary>
        /// Validates activation date
        /// </summary>
        /// <param name="activationDate"></param>
        /// <param name="result"></param>
        private void ValidateActivationDate(DateTime activationDate, ValidationResult result)
        {
            if (activationDate <= DateTime.Today)
            {
                result.AddError("Activation date must be tomorrow or later.");
            }
        }

        /// <summary>
        /// Validates email format if existent
        /// (assuming email is not mandatory)
        /// </summary>
        /// <param name="emails"></param>
        /// <param name="result"></param>
        private void ValidateEmails(ICollection<Email> emails, ValidationResult result)
        {
            foreach (var email in emails)
            {
                if (!_emailValidator.IsValid(email.EmailAddress))
                {
                    result.AddError($"Invalid email format: {email.EmailAddress}");
                }
            }
        }

        /// <summary>
        /// Validates phone number if existent
        /// (assuming phone number is not mandatory)
        /// </summary>
        /// <param name="phones"></param>
        /// <param name="result"></param>
        private void ValidatePhones(ICollection<Phone> phones, ValidationResult result)
        {
            foreach (var phone in phones)
            {
                if (!_phoneValidator.IsValid(phone.PhoneNumber))
                {
                    result.AddError($"Invalid phone number: {phone.PhoneNumber}");
                }
            }
        }
    }
}
