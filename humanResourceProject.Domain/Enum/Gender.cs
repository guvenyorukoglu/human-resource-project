using System.ComponentModel.DataAnnotations;

namespace humanResourceProject.Domain.Enum
{
    public enum Gender
    {
        [Display(Name = "Kadın")]
        Female,
        [Display(Name = "Erkek")]
        Male
    }
}
