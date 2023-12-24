using humanResourceProject.Domain.Entities.Abstract;
using humanResourceProject.Domain.Enum;

namespace humanResourceProject.Domain.Entities.Concrete
{
    public class Advance : IBaseEntity
    {
        public Guid Id { get; set; }
        public string AdvanceNo { get; set; }
        public decimal AmountOfAdvance { get; set; }
        public string? Explanation { get; set; }
        public AdvanceType AdvanceType { get; set; }
        public DateTime ExpiryDate { get; set; }
        public Currency Currency { get; set; }
        public RequestStatus AdvanceStatus { get; set; } = RequestStatus.Pending;

        public AppUser Employee { get; set; }
        public Guid EmployeeId { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public Status Status { get; set; } = Status.Active; 
    }
}
