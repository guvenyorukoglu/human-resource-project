using humanResourceProject.Domain.Entities.Abstract;
using humanResourceProject.Domain.Enum;

namespace humanResourceProject.Domain.Entities.Concrete
{
    public class PossessionLog : IBaseEntity
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public Guid PossessionId { get; set; }
        public DateTime StartDateOfPossession { get; set; }
        public DateTime? EndDateOfPossession { get; set; }
        public AppUser AppUser { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public Status Status { get; set; } = Status.Active;
    }
}
