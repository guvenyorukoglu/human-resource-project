﻿using humanResourceProject.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace humanResourceProject.Models.DTOs
{
    public class UpdateLeaveDTO
    {
        public Guid Id { get; set; }
        public LeaveType LeaveType { get; set; }
        [Required(ErrorMessage = "İzin başlangıç tarihi boş geçilemez!")]
        [DisplayName("İzin başlangıç Tarihi*")]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "İzin bitiş tarihi boş geçilemez!")]
        [DisplayName("İzin bitiş Tarihi*")]
        public DateTime EndDate { get; set; }
        public Guid EmployeeId { get; set; }
        public Guid ManagerId { get; set; }
        public Status Status { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}