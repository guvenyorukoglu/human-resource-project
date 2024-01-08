namespace humanResourceProject.Models.DTOs
{
    public class FireEmployeeDTO
    {
        public Guid EmployeeId { get; set; }
        public string ReasonForTermination { get; set; }
        public DateTime TerminationDate { get; set; }
    }
}
