using System.ComponentModel.DataAnnotations;

namespace humanResourceProject.Domain.Enum
{
    public enum AdvanceType
    {
        [Display(Name = "Maaş Avansı")]
        SalaryAdvance,
        [Display(Name = "İş Avansı")]
        WorkAdvance,
        [Display(Name = "Yıllık İzin Avansı")]
        EarnedLeaveAdvance,
        [Display(Name = "Diğer Avans")]
        OtherAdvance
    }
}
