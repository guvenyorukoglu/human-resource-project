using humanResourceProject.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace humanResourceProject.Models.VMs
{
    public class ProfileEmployeeVM
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? JobTitle { get; set; }
        public string IdentificationNumber { get; set; }
        public BloodGroup BloodGroup { get; set; }
        public DateTime Birthdate { get; set; }
        public Gender Gender { get; set; }
        public string? ManagerName { get; set; }
        public string? ManagerEmail { get; set; }
        public string? ManagerPhoneNumber { get; set; }

    }
}
