﻿using humanResourceProject.Domain.Enum;

namespace humanResourceProject.Models.VMs
{
    public class DashboardExpensesToBeCompletedByManagerVM
    {
        public string ExpenseNo { get; set; }
        public decimal AmountOfExpense { get; set; }
        public DateTime CreateDate { get; set; }
        public RequestStatus ExpenseStatus { get; set; }
    }
}
