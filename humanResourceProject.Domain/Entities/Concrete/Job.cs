using humanResourceProject.Domain.Entities.Abstract;
using humanResourceProject.Domain.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace humanResourceProject.Domain.Entities.Concrete
{
    public class Job : IBaseEntity
    {
        public Job()
        {
            Employees = new HashSet<AppUser>();
        }
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }

        public ICollection<AppUser> Employees { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.Now;
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public Status Status { get; set; } = Status.Active;

    }
}
