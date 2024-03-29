﻿using humanResourceProject.Domain.Entities.Abstract;
using humanResourceProject.Domain.Enum;

namespace humanResourceProject.Domain.Entities.Concrete
{
    public class Leave : IBaseEntity
    {
        public Guid Id { get; set; }
        public string LeaveNo { get; set; }
        public DateTime StartDateOfLeave { get; set; }
        public DateTime EndDateOfLeave { get; set; }
        public LeaveType LeaveType { get; set; }
        public string? Explanation { get; set; }
        public decimal DaysOfLeave { get; set; }
        public RequestStatus LeaveStatus { get; set; } = RequestStatus.Pending;
        public string? RejectReason { get; set; }

        public AppUser Employee { get; set; }
        public Guid EmployeeId { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public Status Status { get; set; } = Status.Active;

        public static string GenerateLeaveNumber(int currentLeaveCount)
        {
            const string prefix = "LEA";

            string sequentialNumber = (currentLeaveCount + 1).ToString("D5");

            string leaveNo = $"{prefix}{DateTime.Now.Year}{sequentialNumber}";

            return leaveNo;
        }
    }
}
