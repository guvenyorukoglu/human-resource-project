﻿using humanResourceProject.Domain.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace humanResourceProject.Models.DTOs
{
    public class LeaveDTO
    {
        //public Guid Id { get; set; }
        public LeaveType LeaveType { get; set; }
        [Required(ErrorMessage = "İzin başlangıç tarihi boş geçilemez!")]
        [DisplayName("İzin başlangıç Tarihi*")]
        public DateTime StartDateOfLeave { get; set; }
        [Required(ErrorMessage = "İzin bitiş tarihi boş geçilemez!")]
        [DisplayName("İzin bitiş Tarihi*")]
        public DateTime EndDateOfLeave { get; set; }
        public Guid EmployeeId { get; set; }
        public Guid ManagerId { get; set; }
        //public Status Status { get; set; } = Status.Active;
        public DateTime CreateDate { get; set; } = DateTime.Now;
        //public DateTime? UpdateDate { get; set; }
        //public RequestStatus LeaveStatus { get; set; }
        public decimal DaysOfLeave { get; set; }
        public string Explanation { get; set; }
    }
}
