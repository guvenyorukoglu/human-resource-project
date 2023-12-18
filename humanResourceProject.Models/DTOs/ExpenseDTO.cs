using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Domain.Enum;

namespace humanResourceProject.Models.DTOs
{
    public class ExpenseDTO
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public decimal AmountOfExpense { get; set; }
        public DateTime? UpdateDate { get; set; }
        public Status Status { get; set; }

    }
}
