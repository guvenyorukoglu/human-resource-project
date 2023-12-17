using System.ComponentModel.DataAnnotations;

namespace humanResourceProject.Domain.Enum
{
    public enum ExpenseType
    {
        [Display(Name = "Konaklama")]
        Accommodation,
        [Display(Name = "Ulaşım")]
        Transportation,
        [Display(Name = "Eğitim")]
        Education,
        [Display(Name = "Yiyecek")]
        Food,
        [Display(Name = "Sağlık")]
        Health,
        [Display(Name = "Diğer")]
        Other
    }
}
