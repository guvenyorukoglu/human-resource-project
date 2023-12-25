using humanResourceProject.Domain.Entities.Abstract;
using humanResourceProject.Domain.Enum;

namespace humanResourceProject.Domain.Entities.Concrete
{
    public class Department : IBaseEntity
    {
        public Department()
        {
            Employees = new HashSet<AppUser>();
        }
        public Guid Id { get; set; }
        public string DepartmentName { get; set; }
        public string? Description { get; set; }

        //public AppUser Manager { get; set; }
        //public Guid? ManagerId { get; set; }
        public Company Company { get; set; }
        public Guid CompanyId { get; set; }

        public ICollection<AppUser> Employees { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public Status Status { get; set; } = Status.Active;
    }
}
