using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace humanResourceProject.DTO.DTOs
{
    public class UserRegisterDTO
    {
        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
        
        public string ConfirmPassword { get; set; }
        
        public string PhoneNumber { get; set;}
        
        public DateTime Birthdate { get; set; }

        public DateTime DateOfEmployment { get; set; }



    }
}
