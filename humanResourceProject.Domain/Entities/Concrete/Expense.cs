﻿using humanResourceProject.Domain.Entities.Abstract;
using humanResourceProject.Domain.Enum;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace humanResourceProject.Domain.Entities.Concrete
{
    public class Expense : IBaseEntity
    {
        public Guid Id { get; set; }
        public string ExpenseNo { get; set; }
        public decimal AmountOfExpense { get; set; }
        public DateTime DateOfExpense { get; set; }
        public ExpenseType ExpenseType { get; set; }
        public string? Explanation { get; set; }
        public Currency Currency { get; set; }
        [NotMapped]
        public IFormFile? UploadPath { get; set; }
        public string? FilePath { get; set; }
        public RequestStatus ExpenseStatus { get; set; } = RequestStatus.Pending;
        public string? RejectReason { get; set; }

        public AppUser Employee { get; set; }
        public Guid EmployeeId { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public Status Status { get; set; } = Status.Active;

        public static string GenerateExpenseNumber(int currentExpenseCount)
        {
            const string prefix = "EXP";

            string sequentialNumber = (currentExpenseCount + 1).ToString("D5");

            string expenseNo = $"{prefix}{DateTime.Now.Year}{sequentialNumber}";

            return expenseNo;
        }
    }
}
