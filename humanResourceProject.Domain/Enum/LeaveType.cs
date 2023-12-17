using System.ComponentModel.DataAnnotations;

namespace humanResourceProject.Domain.Enum
{
    public enum LeaveType
    {
        [Display(Name = "Yıllık İzin")]
        AnnualLeave,
        [Display(Name = "Raporlu İzin")]
        SickLeave,
        [Display(Name = "Evlilik İzni")]
        MarriageLeave,
        [Display(Name = "Doğum İzni")]
        MaternityLeave,
        [Display(Name = "Babalık İzni")]
        PaternityLeave,
        [Display(Name = "Ölüm İzni")]
        BereavementLeave,
        [Display(Name = "Ücretsiz İzin")]
        UnpaidLeave,
        [Display(Name = "Diğer İzin")]
        OtherLeave

    }
}
