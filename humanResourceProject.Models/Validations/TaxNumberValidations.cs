using System;
using System.ComponentModel.DataAnnotations;

namespace humanResourceProject.Models.Validations
{
    public class TaxNumberValidations : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            string checkValue;
            if (value == null)
                return false;

            checkValue = value.ToString();

            int tmp;
            int sum = 0;
            if (value != null && checkValue.Length == 10 && isInt(checkValue))
            {
                int lastDigit = int.Parse(checkValue[9].ToString());
                for (int i = 0; i < 9; i++)
                {
                    int digit = checkValue[i];
                    tmp = (digit + 10 - (i + 1)) % 10;
                    sum = (int)(tmp == 9 ? sum + tmp : sum + ((tmp * (Math.Pow(2, 10 - (i + 1)))) % 9));
                }
                return lastDigit == (10 - (sum % 10)) % 10;
            }
            return false;
        }

        private bool isInt(string? str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                if (i == 0 && str[i] == '-') continue;
                if (!char.IsDigit(str[i])) return false;
            }
            return true;
        }
    }
}

