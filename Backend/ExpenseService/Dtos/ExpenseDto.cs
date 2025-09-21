namespace ExpenseService.Dtos
{
    public class ExpenseDto
    {

        public required string Description { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}