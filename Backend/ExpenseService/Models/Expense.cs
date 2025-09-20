namespace ExpenseService.Models
{
    public class Expense
    {
        public int Id { get; set; }
        public int UserId { get; set; }   // Comes from JWT claims
        public required string Description { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;


    }
}