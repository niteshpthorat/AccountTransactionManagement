using System;
using System.Threading.Tasks;
using AccountTransactionManagement.Data;
using AccountTransactionManagement.Models;
using AccountTransactionManagement.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AccountTransactionManagement.Tests
{
    public class UpdateTransactionServiceTests
    {
        private AppDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite("Data Source=:memory:")
                .Options;

            var context = new AppDbContext(options);
            context.Database.OpenConnection();
            context.Database.EnsureCreated();
            return context;
        }

        [Fact]
        public async Task UpdateTransaction_ValidTransaction_UpdatesTransaction()
        {
            using var context = CreateContext();
            var service = new UpdateTransactionService(context);

            var account = new Account { Name = "Test Account", Number = "123456", CurrentBalance = 1000.00M, OverdraftLimit = 0 };
            await context.Accounts.AddAsync(account);
            await context.SaveChangesAsync();

            var transaction = new Transaction
            {
                Description = "Initial Transaction",
                DebitCredit = "debit",
                Amount = 100.00M,
                AccountId = account.Id
            };

            await context.Transactions.AddAsync(transaction);
            await context.SaveChangesAsync();

            transaction.Description = "Updated Transaction";
            transaction.Amount = 50.00M; // Update amount

            await service.UpdateTransaction(transaction);
            var updatedTransaction = await context.Transactions.FindAsync(transaction.Id);

            Assert.Equal("Updated Transaction", updatedTransaction.Description);
        }
    }
}
