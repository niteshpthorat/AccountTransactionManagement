using System.Threading.Tasks;
using AccountTransactionManagement.Models;

namespace AccountTransactionManagement.Services
{
    public interface ICreateTransactionService
    {
        Task<Transaction> CreateTransaction(Transaction transaction);
    }
}
