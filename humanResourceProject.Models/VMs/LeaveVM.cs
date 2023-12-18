﻿namespace humanResourceProject.Models.VMs
{
    public class LeaveVM
    {
        public Guid Id { get; set; }
        public string LeaveType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid EmployeeId { get; set; }
        public Guid ManagerId { get; set; }
        public string Status { get; set; }
    }
}
