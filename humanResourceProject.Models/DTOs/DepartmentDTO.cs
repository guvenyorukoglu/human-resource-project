using humanResourceProject.Domain.Enum;

namespace humanResourceProject.Models.DTOs
{
    public class DepartmentDTO
    {
        public Guid Id { get; set; }
        public string DepartmentName { get; set; }
        public string? Description { get; set; }
        public Status Status { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
