using System.ComponentModel.DataAnnotations;

namespace humanResourceProject.Domain.Enum
{
    public enum Title
    {
        [Display(Name = "Mr.")]
        Mr,
        [Display(Name = "Ms.")]
        Ms,
        [Display(Name = "Mrs.")]
        Mrs,
        [Display(Name = "Miss")]
        Miss
    }
}
