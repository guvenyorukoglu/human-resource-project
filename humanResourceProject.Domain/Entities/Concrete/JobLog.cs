using humanResourceProject.Domain.Entities.Abstract;
using humanResourceProject.Domain.Enum;

namespace humanResourceProject.Domain.Entities.Concrete
{
    public class JobLog : IBaseEntity
    {
        public Guid Id { get; set; }
        public Guid? JobId { get; set; }
        public Guid EmployeeId { get; set; }
        public DateTime DateOfStart { get; set; }
        public DateTime? DateOfTermination { get; set; }
        public string? ReasonForTermination { get; set; }
        public AppUser AppUser { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public Status Status { get; set; } = Status.Active;
    }
}
