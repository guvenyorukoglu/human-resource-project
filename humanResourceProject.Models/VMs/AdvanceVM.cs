namespace humanResourceProject.Models.VMs
{
    public class AdvanceVM
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeSurname { get; set; }
        public decimal AdvanceAmount { get; set; }
        public DateTime AdvanceDate { get; set; }
        public string Description { get; set; }
    }
}
