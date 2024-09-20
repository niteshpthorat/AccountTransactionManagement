using System.Collections.Generic;
using System.Threading.Tasks;
using AccountTransactionManagement.Models;

namespace AccountTransactionManagement.Services
{
    public interface IReadTransactionService
    {
        Task<List<Transaction>> GetTransactions(int accountId);
    }
}
