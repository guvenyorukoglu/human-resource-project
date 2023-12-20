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
        [DisplayName("Email*")]
        [EmailValidations(ErrorMessage = "Lütfen geçerli bir email adresi giriniz!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Soyisim alanı boş geçilemez!")]
        [DisplayName("Soyisim*")]
        [RegularExpression(@"^[a-zA-ZğĞıİşŞüÜöÖçÇ]*$", ErrorMessage = "Yalnızca alfabetik karakterlere izin verilir.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Adres alanı boş geçilemez!")]
        [DisplayName("Adres*")]
        [StringLength(200, ErrorMessage = "Adres en fazla 200 en az 5 karakter olmalıdır.", MinimumLength = 5)]
        public string Address { get; set; }

        [Required(ErrorMessage = "Telefonu alanı boş geçilemez!")]
        [DisplayName("Telefon*")]
        [PhoneValidations(ErrorMessage = "Lütfen geçerli bir telefon numarası giriniz!")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Cinsiyet alanı boş geçilemez!")]
        [DisplayName("Cinsiyet*")]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = "Meslek alanı boş geçilemez!")]
        [DisplayName("Meslek*")]
        [RegularExpression(@"^[a-zA-ZğĞıİşŞüÜöÖçÇ\s]*$", ErrorMessage = "Yalnızca alfabetik karakterlere izin verilir.")]
        public string Job { get; set; }
        public Guid DepartmentId { get; set; }
        public List<DepartmentVM>? Departments { get; set; }
        public Guid JobId { get; set; }
        public List<JobVM>? Jobs { get; set; }
        public DateTime UpdateDate => DateTime.Now;
        public Status Status => Status.Modified;
        public RequestStatus RequestStatus => RequestStatus.Pending;

    }
}
