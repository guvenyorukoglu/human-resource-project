using humanResourceProject.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace humanResourceProject.DTO.DTOs
{
    public class LoginDTO
    {
        [Display(Name = "Kullanıcı Adı")]
        [Required(ErrorMessage = "Kullanıcı adı boş olamaz")]
        [MaxLength(50)]
        [MinLength(3, ErrorMessage = "Kullanıcı Adı 3 karakterden az olamaz")]
        public string UserName { get; set; }


        [Display(Name = "Şifre")]
        [Required(ErrorMessage = "Şifre alanı boş olamaz")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public Status status { get; set; }
    }
}
