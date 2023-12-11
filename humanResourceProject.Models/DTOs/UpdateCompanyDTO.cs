using humanResourceProject.Domain.Enum;

namespace humanResourceProject.Models.DTOs
{
    public class UpdateCompanyDTO
    {
        public Guid Id { get; set; }
        public string? CompanyName { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public int NumberOfEmployees { get; set; }
        public DateTime UpdateDate => DateTime.Now;
        public Status Status => Status.Modified;

    }
}
