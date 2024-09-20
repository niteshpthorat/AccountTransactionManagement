namespace AccountTransactionManagement.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty; 
        public string DebitCredit { get; set; } = string.Empty; 
        public decimal Amount { get; set; }
        public int AccountId { get; set; }
        public Account Account { get; set; } = null!; 
    }
}
