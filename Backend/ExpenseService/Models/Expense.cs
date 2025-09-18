namespace ExpenseService.Models
{
    public class Expenese
    {
        public int Id { get; set; }
        public int UserId { get; set; }   // Comes from JWT claims
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;


    }
}