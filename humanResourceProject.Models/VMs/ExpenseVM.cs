using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Domain.Enum;
using Microsoft.AspNetCore.Http;
using System.ComponentModel;

namespace humanResourceProject.Models.VMs
{
    public class ExpenseVM
    {
        public Guid Id { get; set; }
        public string Explanation { get; set; }
        public decimal AmountOfExpense { get; set; }
        public Currency Currency { get; set; }
        public Guid EmployeeId { get; set; }
        public Guid ManagerId { get; set; }
        public Guid DepartmentId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeSurname { get; set; }
        public RequestStatus ExpenseStatus { get; set; }
        public string? FilePath { get; set; }
        [DisplayName("Masraf Fotoğrafı")]
        public IFormFile? UploadPath { get; set; }
        public DateTime DateOfExpense { get; set; }
        public ExpenseType ExpenseType { get; set; }
        public DateTime CreateDate { get; set; }

    }
}
