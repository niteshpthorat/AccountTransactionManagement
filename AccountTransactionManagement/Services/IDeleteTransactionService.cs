using System.Threading.Tasks;

namespace AccountTransactionManagement.Services
{
    public interface IDeleteTransactionService
    {
        Task DeleteTransaction(int transactionId);
    }
}
