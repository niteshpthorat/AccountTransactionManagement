using Microsoft.EntityFrameworkCore;
using AccountTransactionManagement.Models;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountTransactionManagement.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .HasMany(a => a.Transactions)
                .WithOne(t => t.Account)
                .HasForeignKey(t => t.AccountId);
        }

        public async Task SeedDataAsync(string jsonFilePath)
        {
            var jsonData = await File.ReadAllTextAsync(jsonFilePath);
            var seedData = JsonConvert.DeserializeObject<SeedData>(jsonData);

            foreach (var account in seedData.Accounts)
            {
                await Accounts.AddAsync(account);
            }

            foreach (var transaction in seedData.Transactions)
            {
                var account = await Accounts.FindAsync(transaction.AccountId);
                if (account != null)
                {
                    if (transaction.DebitCredit == "debit")
                    {
                        account.CurrentBalance -= transaction.Amount;
                    }
                    else if (transaction.DebitCredit == "credit")
                    {
                        account.CurrentBalance += transaction.Amount;
                    }

                    await Transactions.AddAsync(transaction);
                }
            }

            await SaveChangesAsync();
        }

    }
}
