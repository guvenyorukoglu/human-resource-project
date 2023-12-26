using System.ComponentModel.DataAnnotations;

namespace humanResourceProject.Models.Validations
{
    public class NumberOfEmployeesValidation : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is int numberOfEmployees)
                return numberOfEmployees >= 1 && numberOfEmployees < 2500000;
            else
                return false;
        }
    }
}
