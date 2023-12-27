
using System.ComponentModel.DataAnnotations;

namespace humanResourceProject.Models.Validations
{
    public class DateTimeValidations : ValidationAttribute
    {
        public class ExpenseMinDateTimeValidations : ValidationAttribute
        {
            public override bool IsValid(object value)
            {
                if ((DateTime)value < DateTime.Now.AddMonths(-1))
                {
                    return false;
                }
                return true;
            }
        }

        public class ExpenseMaxDateTimeValidations : ValidationAttribute
        {
            public override bool IsValid(object value)
            {
                if ((DateTime)value > DateTime.Now)
                {
                    return false;
                }
                return true;
            }
        }

        public class LeaveDateTimeValidations : ValidationAttribute
        {
            public override bool IsValid(object value)
            {
                if ((DateTime)value < DateTime.Now.AddMonths(-3) || (DateTime)value > DateTime.Now.AddMonths(3))
                {
                    return false;
                }
                return true;
            }
        }

        public class AdvanceMinDateTimeValidations : ValidationAttribute
        {
            public override bool IsValid(object value)
            {
                if ((DateTime)value < DateTime.Now)
                {
                    return false;
                }
                return true;
            }
        }

    }
}
