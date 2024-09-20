using System.Collections.Generic;
using System.Threading.Tasks;
using AccountTransactionManagement.Data;
using AccountTransactionManagement.Models;
using AccountTransactionManagement.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AccountTransactionManagement.Tests
{
    public class ReadTransactionServiceTests
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
        public async Task GetTransactions_ValidAccountId_ReturnsTransactions()
        {
            using var context = CreateContext();
            var service = new ReadTransactionService(context);

            var account = new Account { Name = "Test Account", Number = "123456", CurrentBalance = 1000.00M, OverdraftLimit = 0 };
            await context.Accounts.AddAsync(account);
            await context.SaveChangesAsync();

            var transaction = new Transaction
            {
                Description = "Test Transaction",
                DebitCredit = "debit",
                Amount = 100.00M,
                AccountId = account.Id
            };

            await context.Transactions.AddAsync(transaction);
            await context.SaveChangesAsync();

            var transactions = await service.GetTransactions(account.Id);
            Assert.Single(transactions);
        }
    }
}
