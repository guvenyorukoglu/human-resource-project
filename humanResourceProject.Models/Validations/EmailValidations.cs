using System.ComponentModel.DataAnnotations;

namespace humanResourceProject.Models.Validations
{
    public class EmailValidations : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            string checkValue;
            if (value == null)
                return false;

            checkValue = value.ToString();

            if (checkValue.Where(cv => cv == ' ').ToList().Count > 0)
                return false;

            if (checkValue.Split("@").Count() > 2)
            {
                return false;
            }

            if (checkValue.EndsWith("@bilgeadam.com") || checkValue.EndsWith("@gmail.com") || checkValue.EndsWith("@hotmail.com") || checkValue.EndsWith("@outlook.com") || checkValue.EndsWith("@yahoo.com") || checkValue.EndsWith("@bilgeadamboost.com"))
                return true;

            return false;
        }
    }
}
