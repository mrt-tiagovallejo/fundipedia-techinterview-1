using System.Text.RegularExpressions;

namespace Fundipedia.TechnicalInterview.Domain.Validators
{
    public class PhoneValidator : IPhoneValidator
    {
        public bool IsValid(string phone)
        {
            return Regex.IsMatch(phone, @"^\d{1,10}$");
        }
    }
}
