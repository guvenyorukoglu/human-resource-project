using System.ComponentModel.DataAnnotations;

namespace humanResourceProject.Models.Validations
{
    public class DaysOfLeaveValidations : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is decimal daysOfLeave)
            {
                if (daysOfLeave % 0.5m != 0)
                {
                    return false;
                }
                if (daysOfLeave > 180 || daysOfLeave < 0)
                {
                    return false;
                }
               
                return true;
            }

            return false;
        }
    }
}
