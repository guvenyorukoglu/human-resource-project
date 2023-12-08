namespace humanResourceProject.VM.VMs
{
    public class CompanyVM
    {
        public Guid Id { get; set; }
        public string CompanyName { get; set; }
        public int NumberOfEmployees { get; set; }
        public string TaxNumber { get; set; }
        public string TaxOffice { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
    }
}
