namespace ExpenseService.Dtos
{
    public class ExpeneseDto
    {

        public string Description { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}