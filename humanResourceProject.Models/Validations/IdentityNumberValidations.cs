using System.ComponentModel.DataAnnotations;

namespace humanResourceProject.Models.Validations
{
    public class IdentityNumberValidations : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            string checkValue;
            if (value == null)
                return false;

            checkValue = value.ToString();
            int toplam = 0; int toplam2 = 0; int toplam3 = 0;

            if (checkValue.Length == 11)
            {
                if (Convert.ToInt32(checkValue[0].ToString()) != 0)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        toplam = toplam + Convert.ToInt32(checkValue[i].ToString());
                        if (i % 2 == 0)
                        {
                            if (i != 10)
                            {
                                toplam2 = toplam2 + Convert.ToInt32(checkValue[i].ToString());
                            }

                        }
                        else
                        {
                            if (i != 9)
                            {
                                toplam3 = toplam3 + Convert.ToInt32(checkValue[i].ToString());
                            }
                        }
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

            if (((toplam2 * 7) - toplam3) % 10 == Convert.ToInt32(checkValue[9].ToString()) && toplam % 10 == Convert.ToInt32(checkValue[10].ToString()))
                return true;
            else
                return false;
        }
    }
}
