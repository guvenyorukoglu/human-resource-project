using humanResourceProject.Domain.Entities.Abstract;
using humanResourceProject.Domain.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace humanResourceProject.Domain.Entities.Concrete
{
    public class AppUser : IdentityUser<Guid>, IBaseEntity
    {
        public AppUser()
        {
            Expenses = new HashSet<Expense>();
            Advances = new HashSet<Advance>();
            Leaves = new HashSet<Leave>();
            Supervisee = new HashSet<AppUser>();

        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? MiddleName { get; set; }
        [NotMapped]
        public string FullName
        {
            get
            {
                if (MiddleName != null)
                {
                    return $"{FirstName} {MiddleName} {LastName}";
                }
                return $"{FirstName} {LastName}";
            }
        }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string IdentificationNumber { get; set; }
        public decimal EarnedLeaveDays { get; set; }
        public decimal RemainingLeaveDays { get; set; }
        public BloodGroup BloodGroup { get; set; }
        public DateTime Birthdate { get; set; }
        public Gender Gender { get; set; }
        public string? ImagePath { get; set; }
        [NotMapped]
        public IFormFile UploadPath { get; set; }

        public ICollection<Expense>? Expenses { get; set; }
        public ICollection<Advance>? Advances { get; set; }
        public ICollection<Leave>? Leaves { get; set; }
        public ICollection<JobLog>? JobLogs { get; set; }
        public ICollection<PossessionLog>? PossessionLogs { get; set; }

        public AppUser Manager { get; set; }
        public Guid? ManagerId { get; set; }

        [JsonIgnore]
        public ICollection<AppUser>? Supervisee { get; set; }
        public Company Company { get; set; }
        public Guid CompanyId { get; set; }

        //public Job Job { get; set; }
        public Guid? JobId { get; set; }

        //public Department Department { get; set; }
        public Guid? DepartmentId { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public Status Status { get; set; } = Status.Inactive;
    }
}
