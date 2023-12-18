namespace humanResourceProject.Models.DTOs
{
    public class ExpenseDTO
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
    }
}
