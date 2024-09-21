using System.Collections.Generic;
using AccountTransactionManagement.Models;

namespace AccountTransactionManagement.Data
{
    public class SeedData
    {
        public List<Account>? Accounts { get; set; } = new List<Account>();
        public List<Transaction>? Transactions { get; set; } = new List<Transaction>();

    }
}
