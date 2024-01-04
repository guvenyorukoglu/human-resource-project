using humanResourceProject.Domain.Entities.Abstract;
using humanResourceProject.Domain.Enum;

namespace humanResourceProject.Domain.Entities.Concrete
{
    public class Possession : IBaseEntity
    {
        public Guid Id { get; set; }
        public string Barcode { get; set; }
        public Guid CompanyId { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string? Details { get; set; }
        public PossessionType PossessionType { get; set; }
        public Company Company { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public Status Status { get; set; } = Status.Active;
    }
}
