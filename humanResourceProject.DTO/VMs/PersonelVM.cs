using humanResourceProject.Domain.Enum;

namespace humanResourceProject.DTO.VMs
{
    public class PersonelVM
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? MiddleName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Title Title { get; set; }
        public string Job { get; set; }
    }
}
