namespace AccountTransactionManagement.Models
{
    public class Transaction
    {
        public int Id { get; set; }  // "id"
        public string Description { get; set; }  // "description"
        public string DebitCredit { get; set; }  // "debit_credit"
        public decimal Amount { get; set; }  // "amount"
        public int AccountId { get; set; }  // "account_id"

        public Account Account { get; set; }
    }
}
