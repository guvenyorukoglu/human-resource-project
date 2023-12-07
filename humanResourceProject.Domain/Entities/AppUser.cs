using humanResourceProject.Domain.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace humanResourceProject.Domain.Entities
{
    public class AppUser : IdentityUser<Guid>, IBaseEntity
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string? MiddleName  { get; set; }

        public string Email { get; set; }
        public string Adress { get; set; }
        public string PhoneNumber { get; set; }
        public string IdentificationNumber { get; set; }

        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public DateTime Birthdate { get; set; }
        public string Title { get; set; }
        public string Job { get; set; }
        public string ImagePath { get; set; }

        
        [NotMapped]
        public IFormFile UploadPath { get; set; }


        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public Status Status { get; set; }

        public CompanyInformation companyInformation { get; set; }
        public int CompanyId { get; set; }
    }
}
