using System;
using System.Threading.Tasks;
using AccountTransactionManagement.Data;
using AccountTransactionManagement.Models;
using AccountTransactionManagement.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AccountTransactionManagement.Tests
{
    public class CreateTransactionServiceTests
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
        public async Task CreateTransaction_ValidTransaction_AddsTransaction()
        {
            using var context = CreateContext();
            var service = new CreateTransactionService(context);

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

            await service.CreateTransaction(transaction);
            Assert.Equal(1, await context.Transactions.CountAsync());
            Assert.Equal(900.00M, account.CurrentBalance); 
        }

        [Fact]
        public async Task CreateTransaction_ExceedingOverdraftLimit_ThrowsException()
        {
            using var context = CreateContext();
            var service = new CreateTransactionService(context);

            var account = new Account { Name = "Test Account", Number = "123456", CurrentBalance = 100.00M, OverdraftLimit = 50 };
            await context.Accounts.AddAsync(account);
            await context.SaveChangesAsync();

            var transaction = new Transaction
            {
                Description = "Overdraft Transaction",
                DebitCredit = "debit",
                Amount = 200.00M, // Exceeds overdraft limit
                AccountId = account.Id
            };

            await Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateTransaction(transaction));
        }
    }
}
