namespace AccountTransactionManagement.Models
{
    public class Account
    {
        public int Id { get; set; }  // Matches "id" in JSON
        public string Name { get; set; }  // "name"
        public string Number { get; set; }  // "number"
        public decimal CurrentBalance { get; set; }  // "current_balance"
        public decimal OverdraftLimit { get; set; }  // "overdraft_limit"

        public ICollection<Transaction> Transactions { get; set; }
    }
}
