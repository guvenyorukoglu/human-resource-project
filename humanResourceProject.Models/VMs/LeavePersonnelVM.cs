using humanResourceProject.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace humanResourceProject.Models.VMs
{
    public class LeavePersonnelVM
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Başlangıç Tarihi boş geçilemez.")]
        [DisplayName("Başlangıç Tarihi*")]
        public DateTime StartDateOfLeave { get; set; }

        [Required(ErrorMessage = "Bitiş Tarihi boş geçilemez.")]
        [DisplayName("Bitiş Tarihi*")]
        public DateTime EndDateOfLeave { get; set; }

        [Required(ErrorMessage = "İzin Türü boş geçilemez.")]
        [DisplayName("İzin Türü*")]
        public LeaveType LeaveType { get; set; }

        [DisplayName("Açıklama")]
        public string Explanation { get; set; }

        [Required(ErrorMessage = "İzin Gün Sayısı boş geçilemez.")]
        [DisplayName("İzin Gün Sayısı*")]
        public decimal DaysOfLeave { get; set; }
        public RequestStatus LeaveStatus { get; set; }


    }
}
