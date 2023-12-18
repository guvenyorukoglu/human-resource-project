using humanResourceProject.Domain.Enum;

namespace humanResourceProject.Models.VMs
{
    public class DepartmentVM
    {
        public Guid Id { get; set; }
        public string DepartmentName { get; set; }
        public string? Description { get; set; }
        public Status Status { get; set; }
    }
}
