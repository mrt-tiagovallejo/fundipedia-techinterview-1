using System.Text.RegularExpressions;

namespace Fundipedia.TechnicalInterview.Domain.Validators
{
    public class EmailValidator : IEmailValidator
    {
        public bool IsValid(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }
    }
}
