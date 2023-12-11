using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace humanResourceProject.Models.Validations
{
    public class PasswordValidations : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            var password = value as string;

            if (string.IsNullOrEmpty(password) || password.Length < 6)
                return false;

            Regex hasUpperCase = new Regex(@"[A-Z]+");
            Regex hasLowerCase = new Regex(@"[a-z]+");
            Regex hasDigit = new Regex(@"[0-9]+");
            Regex hasSpecialChar = new Regex(@"[!@#$%^&*()_+[\]{};':\""|<>/?]+");


            var isValid = hasUpperCase.IsMatch(password) &&
                              hasLowerCase.IsMatch(password) &&
                              hasDigit.IsMatch(password) &&
                              hasSpecialChar.IsMatch(password);

            return isValid;
        }
    }
}

