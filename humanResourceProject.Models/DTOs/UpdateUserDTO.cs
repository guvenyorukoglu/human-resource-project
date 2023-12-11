using humanResourceProject.Domain.Enum;

using humanResourceProject.Models.VMs;

using humanResourceProject.Models.Validations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;


namespace humanResourceProject.Models.DTOs
{
    public class UpdateUserDTO
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Email alanı boş geçilemez!")]
        [DisplayName("Email")]
        [EmailValidations(ErrorMessage = "Lütfen geçerli bir email adresi giriniz!")]
        public string Email { get; set; }
        public string LastName { get; set; }
        public string Adress { get; set; }
        [Required(ErrorMessage = "Telefonu alanı boş geçilemez!")]
        [DisplayName("Telefon")]
        [PhoneValidations(ErrorMessage = "Lütfen geçerli bir telefon numarası giriniz!")]
        public string PhoneNumber { get; set; }
        public Title Title { get; set; }
        public string Job { get; set; }
        public Guid CompanyId { get; set; }
        public List<CompanyVM>? Companies { get; set; }
        public DateTime UpdateDate => DateTime.Now;
        public Status Status => Status.Modified;

    }
}
