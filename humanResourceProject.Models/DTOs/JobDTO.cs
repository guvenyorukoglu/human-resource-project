using humanResourceProject.Domain.Enum;

namespace humanResourceProject.Models.DTOs
{
    public class JobDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime? UpdateDate { get; set; }
        public Status Status { get; set; }
    }
}
