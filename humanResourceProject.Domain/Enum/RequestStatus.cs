using System.ComponentModel.DataAnnotations;

namespace humanResourceProject.Domain.Enum
{
    public enum RequestStatus
    {
        [Display(Name = "Onay Bekliyor")]
        Pending = 0,
        [Display(Name = "Onaylandı")]
        Approved = 1,
        [Display(Name = "Reddedildi")]
        Rejected = 2
    }
}
