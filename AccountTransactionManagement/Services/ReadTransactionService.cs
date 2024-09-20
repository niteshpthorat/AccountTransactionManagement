using System.Collections.Generic;
using System.Threading.Tasks;
using AccountTransactionManagement.Data;
using AccountTransactionManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountTransactionManagement.Services
{
    public class ReadTransactionService : IReadTransactionService
    {
        private readonly AppDbContext _context;

        public ReadTransactionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Transaction>> GetTransactions(int accountId)
        {
            return await _context.Transactions
                .Where(t => t.AccountId == accountId)
                .ToListAsync();
        }
    }
}
