namespace ExpenseService.Events
{
    public class ExpenseCreatedEvent
    {
        public int ExpenseId { get; set; }
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
    }
}
