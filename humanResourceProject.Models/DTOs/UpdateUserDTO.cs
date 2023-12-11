using humanResourceProject.Domain.Enum;
using humanResourceProject.Models.VMs;

namespace humanResourceProject.Models.DTOs
{
    public class UpdateUserDTO
    {
        public Guid Id { get; set; }
        public string? Email { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public Title? Title { get; set; }
        public string? Job { get; set; }
        public Guid CompanyId { get; set; }
        public List<CompanyVM>? Companies { get; set; }
        public DateTime UpdateDate => DateTime.Now;
        public Status Status => Status.Modified;
    }
}
