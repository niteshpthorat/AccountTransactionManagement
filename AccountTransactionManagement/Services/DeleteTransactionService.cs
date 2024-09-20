using System;
using System.Threading.Tasks;
using AccountTransactionManagement.Data;

namespace AccountTransactionManagement.Services
{
    public class DeleteTransactionService : IDeleteTransactionService
    {
        private readonly AppDbContext _context;

        public DeleteTransactionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task DeleteTransaction(int transactionId)
        {
            var transaction = await _context.Transactions.FindAsync(transactionId);
            if (transaction == null)
            {
                throw new InvalidOperationException("Transaction not found.");
            }

            var account = await _context.Accounts.FindAsync(transaction.AccountId);
            if (account == null)
            {
                throw new InvalidOperationException("Associated account not found.");
            }

            account.CurrentBalance -= transaction.DebitCredit == "debit" ? -transaction.Amount : transaction.Amount;

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
        }
    }
}
