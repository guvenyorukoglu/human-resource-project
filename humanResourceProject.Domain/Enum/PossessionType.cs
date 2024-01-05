using System.ComponentModel.DataAnnotations;

namespace humanResourceProject.Domain.Enum
{
    public enum PossessionType
    {
        [Display(Name = "Bilgisayar")]
        Computer,
        [Display(Name = "Telefon")]
        Phone,
        [Display(Name = "Tablet")]
        Tablet,
        [Display(Name = "Otomobil")]
        Automobile,
        [Display(Name = "Diğer")]
        Other
    }
}
