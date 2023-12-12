using System.ComponentModel.DataAnnotations;

namespace humanResourceProject.Models.Validations
{
    public class BirthdateValidations : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is DateTime birthdate)
            {
                DateTime currentDate = DateTime.Today;

                int age = currentDate.Year - birthdate.Year;

                if (currentDate < birthdate.AddYears(age))
                {
                    age--;
                }

                return age >= 18;
            }

            return false;
        }
    }
}
