using System.Threading.Tasks;
using AccountTransactionManagement.Models;

namespace AccountTransactionManagement.Services
{
    public interface IUpdateTransactionService
    {
        Task UpdateTransaction(Transaction transaction);
    }
}
