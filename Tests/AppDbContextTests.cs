using System;
using System.Linq;
using System.Threading.Tasks;
using AccountTransactionManagement.Data;
using AccountTransactionManagement.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AccountTransactionManagement.Tests
{
    public class AppDbContextTests
    {
        [Fact]
        public async Task CanAddAccountAndTransaction()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite("Data Source=:memory:") // Use in-memory database for testing
                .Options;

            // Act
            using (var context = new AppDbContext(options))
            {
                context.Database.OpenConnection(); // Open connection
                context.Database.EnsureCreated();  // Ensure the database is created

                var account = new Account
                {
                    Name = "Test Account",
                    Number = "123456",
                    CurrentBalance = 1000.00M,
                    OverdraftLimit = 0
                };

                await context.Accounts.AddAsync(account);
                await context.SaveChangesAsync(); // Save changes to create the Accounts table

                var transaction = new Transaction
                {
                    Description = "Test Transaction",
                    DebitCredit = "debit",
                    Amount = 100.00M,
                    AccountId = account.Id
                };

                await context.Transactions.AddAsync(transaction);
                await context.SaveChangesAsync();
            }

            // Assert
            using (var context = new AppDbContext(options))
            {
                var accounts = await context.Accounts.ToListAsync();
                var transactions = await context.Transactions.ToListAsync();

                Assert.Single(accounts);
                Assert.Single(transactions);
                Assert.Equal("Test Account", accounts[0].Name);
                Assert.Equal("Test Transaction", transactions[0].Description);
            }
        }
    }
}
