using humanResourceProject.Domain.Enum;

namespace humanResourceProject.Models.VMs
{
    public class AppUserVM
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Pozisyon { get; set; }
        public string Departman { get; set; }
        public Gender Gender { get; set; }
        public decimal EarnedLeaveDays { get; set; }
        public decimal RemainingLeaveDays { get; set; }
    }
}
