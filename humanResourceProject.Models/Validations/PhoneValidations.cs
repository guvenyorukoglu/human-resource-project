using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace humanResourceProject.Models.Validations
{
    public class PhoneValidations : ValidationAttribute
    {
        public const string pattern = @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$";

        public override bool IsValid(object? value)
        {
            string checkValue;
            if (value == null)
                return false;

            checkValue = value.ToString();
            if (checkValue != null) 
                return Regex.IsMatch(checkValue, pattern);
            else
                return false;
        }
    }
}
