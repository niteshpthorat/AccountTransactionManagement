using System;
using System.Threading.Tasks;
using AccountTransactionManagement.Data;
using AccountTransactionManagement.Models;

namespace AccountTransactionManagement.Services
{
    public class CreateTransactionService : ICreateTransactionService
    {
        private readonly AppDbContext _context;

        public CreateTransactionService(AppDbContext context)
        {
            _context = context;
        }
        

        public async Task<Transaction> CreateTransaction(Transaction transaction)
        {
            var account = await _context.Accounts.FindAsync(transaction.AccountId);
            if (account == null)
            {
                throw new InvalidOperationException("Account not found.");
            }

            if (transaction.DebitCredit == "debit" && account.CurrentBalance - transaction.Amount < -account.OverdraftLimit)
            {
                throw new InvalidOperationException("Transaction exceeds overdraft limit.");
            }

            account.CurrentBalance += transaction.DebitCredit == "debit" ? -transaction.Amount : transaction.Amount;

            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }
    }
}
