using System.Collections.Generic;

namespace AccountTransactionManagement.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; 
        public string Number { get; set; } = string.Empty; 
        public decimal CurrentBalance { get; set; }
        public decimal OverdraftLimit { get; set; }
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>(); 
    }
}
