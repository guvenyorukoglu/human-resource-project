using humanResourceProject.Domain.Enum;

namespace humanResourceProject.Models.VMs
{
    public class PossessionVM
    {
        public Guid Id { get; set; }
        public string Barcode { get; set; }
        public string Brand { get; set; }
        public string PossessionModel { get; set; }
        public string? Details { get; set; }
        public PossessionType PossessionType { get; set; }
        public string ToWhomItBelongs { get; set; }
        public DateTime StartDateOfPossession { get; set; }
        public DateTime? EndDateOfPossession { get; set; }
    }
}
