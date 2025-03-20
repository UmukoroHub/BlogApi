using System.ComponentModel.DataAnnotations;

namespace BlogApi.Attribute
{
    public class AllowedEmailDomainAttribute : ValidationAttribute
    {
        private readonly string[] _allowedDomains;

        public AllowedEmailDomainAttribute(string allowedDomains)
        {
            _allowedDomains = allowedDomains.Split(','); // Allow multiple domains
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is string email && email.Contains('@'))
            {
                var domain = email.Split('@')[1];
                if (_allowedDomains.Any(d => domain.EndsWith(d, StringComparison.OrdinalIgnoreCase)))
                {
                    return ValidationResult.Success;
                }

                return new ValidationResult($"The email domain '{domain}' is not allowed.");
            }

            return new ValidationResult("Invalid email format.");
        }
    }
}
