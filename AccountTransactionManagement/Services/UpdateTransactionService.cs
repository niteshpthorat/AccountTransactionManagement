using System;
using System.Threading.Tasks;
using AccountTransactionManagement.Data;
using AccountTransactionManagement.Models;

namespace AccountTransactionManagement.Services
{
    public class UpdateTransactionService : IUpdateTransactionService
    {
        private readonly AppDbContext _context;

        public UpdateTransactionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task UpdateTransaction(Transaction transaction)
        {
            var existingTransaction = await _context.Transactions.FindAsync(transaction.Id);
            if (existingTransaction == null)
            {
                throw new InvalidOperationException("Transaction not found.");
            }

            var account = await _context.Accounts.FindAsync(existingTransaction.AccountId);
            if (account == null)
            {
                throw new InvalidOperationException("Associated account not found.");
            }

            account.CurrentBalance -= existingTransaction.DebitCredit == "debit" ? existingTransaction.Amount : -existingTransaction.Amount;

            existingTransaction.Description = transaction.Description;
            existingTransaction.DebitCredit = transaction.DebitCredit;
            existingTransaction.Amount = transaction.Amount;

            if (transaction.DebitCredit == "debit" && account.CurrentBalance - transaction.Amount < -account.OverdraftLimit)
            {
                throw new InvalidOperationException("Transaction exceeds overdraft limit after update.");
            }

            account.CurrentBalance += transaction.DebitCredit == "debit" ? -transaction.Amount : transaction.Amount;

            await _context.SaveChangesAsync();
        }
    }
}
