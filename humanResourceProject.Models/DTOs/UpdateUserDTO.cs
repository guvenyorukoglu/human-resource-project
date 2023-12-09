using humanResourceProject.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace humanResourceProject.Models.DTOs
{
    public class UpdateUserDTO
    {
        public string Email { get; set; }
        public string LastName { get; set; }
        public string Adress { get; set; }
        public string PhoneNumber { get; set; }
        public Title Title { get; set; }
        public string Job { get; set; }
        public DateTime UpdateDate { get; set; }
        public Status status { get; set; }
    }
}
