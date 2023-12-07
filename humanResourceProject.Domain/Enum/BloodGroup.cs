using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace humanResourceProject.Domain.Enum
{
    public enum BloodGroup
    {
        [Display(Name = "AB Rh+")]
        ABPositive,
        [Display(Name = "AB Rh-")]
        ABNegative,
        [Display(Name = "A Rh+")]
        APositive,
        [Display(Name = "A Rh-")]
        ANegative,
        [Display(Name = "B Rh+")]
        BPositive,
        [Display(Name = "B Rh-")]
        BNegative,
        [Display(Name = "O Rh+")]
        OPositive,
        [Display(Name = "O Rh-")]
        ONegative
    }
}
