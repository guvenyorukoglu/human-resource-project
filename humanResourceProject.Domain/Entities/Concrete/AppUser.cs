using humanResourceProject.Domain.Entities.Abstract;
using humanResourceProject.Domain.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace humanResourceProject.Domain.Entities.Concrete
{
    public class AppUser : IdentityUser<Guid>, IBaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? MiddleName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string IdentificationNumber { get; set; }
        public BloodGroup BloodGroup { get; set; }
        public DateTime Birthdate { get; set; }
        public Title Title { get; set; }
        public string Job { get; set; }
        public string? ImagePath { get; set; }


        [NotMapped]
        public IFormFile UploadPath { get; set; }


        public DateTime CreateDate { get; set; } = DateTime.Now;
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public Status Status { get; set; } = Status.Inactive;

        public Company Company { get; set; }
        public Guid CompanyId { get; set; }
    }
}
