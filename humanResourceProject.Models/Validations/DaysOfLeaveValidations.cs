using System.ComponentModel.DataAnnotations;

namespace humanResourceProject.Models.Validations
{
    public class DaysOfLeaveValidations : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is decimal daysOfLeave)
            {
                return daysOfLeave >= 0.5m;
            }

            return false;
        }
    }
}
