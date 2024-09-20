using System.Threading.Tasks;
using AccountTransactionManagement.Data;
using AccountTransactionManagement.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AccountTransactionManagement.Tests
{
    public class AppDbContextTests
    {
        private DbContextOptions<AppDbContext> CreateNewContextOptions()
        {
            // Create a new in-memory database for each test
            return new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite("Data Source=:memory:")
                .Options;
        }

        private async Task<AppDbContext> CreateContextAsync()
        {
            var options = CreateNewContextOptions();
            var context = new AppDbContext(options);

            context.Database.OpenConnection();
            context.Database.EnsureCreated();
            return context;
        }

        [Fact]
        public async Task CanAddAccountAndTransaction()
        {
            // Arrange
            using var context = await CreateContextAsync();

            var account = new Account
            {
                Name = "Test Account",
                Number = "123456",
                CurrentBalance = 1000.00M,
                OverdraftLimit = 0
            };

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

            // Assert
            var accounts = await context.Accounts.ToListAsync();
            var transactions = await context.Transactions.ToListAsync();

            Assert.Single(accounts);
            Assert.Single(transactions);
            Assert.Equal("Test Account", accounts[0].Name);
            Assert.Equal("Test Transaction", transactions[0].Description);
        }

        [Fact]
        public async Task CanAddMultipleAccounts()
        {
            // Arrange
            using var context = await CreateContextAsync();

            var account1 = new Account { Name = "Account 1", Number = "111", CurrentBalance = 500.00M, OverdraftLimit = 0 };
            var account2 = new Account { Name = "Account 2", Number = "222", CurrentBalance = 1000.00M, OverdraftLimit = 100 };

            await context.Accounts.AddRangeAsync(account1, account2);
            await context.SaveChangesAsync();

            // Assert
            var accounts = await context.Accounts.ToListAsync();
            Assert.Equal(2, accounts.Count);
        }
    }
}
