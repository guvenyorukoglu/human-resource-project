﻿using humanResourceProject.Domain.Entities.Abstract;
using humanResourceProject.Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace humanResourceProject.Domain.Entities.Concrete
{
    public class Advance : IBaseEntity
    {
        public Guid Id { get; set; }
        public string AdvanceNo { get; set; }
        public decimal AmountOfAdvance { get; set; }
        public string? Explanation { get; set; }
        public AdvanceType AdvanceType { get; set; }
        public DateTime ExpiryDate { get; set; }
        public Currency Currency { get; set; }
        public RequestStatus AdvanceStatus { get; set; } = RequestStatus.Pending;
        [StringLength(500, ErrorMessage = "En fazla 500 karakter olmalıdır.")]
        public string? RejectReason { get; set; }

        public AppUser Employee { get; set; }
        public Guid EmployeeId { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public Status Status { get; set; } = Status.Active;

        public static string GenerateAdvanceNumber(int currentAdvanceCount)
        {
            const string prefix = "ADV";

            string sequentialNumber = (currentAdvanceCount + 1).ToString("D5");

            string advanceNo = $"{prefix}{DateTime.Now.Year}{sequentialNumber}";

            return advanceNo;
        }
    }
}
