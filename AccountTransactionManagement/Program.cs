using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AccountTransactionManagement.Data;
using AccountTransactionManagement.Models;
using AccountTransactionManagement.Services;

namespace AccountTransactionManagement
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Configure the database context
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlite("Data Source=AccountTransactionDB.db");

            using (var context = new AppDbContext(optionsBuilder.Options))
            {
                context.Database.EnsureCreated();

                // Seed the data if the database is empty
                await context.SeedDataAsync("../Data.json");

                // Initialize services with correct context
                var createTransactionService = new CreateTransactionService(context);
                var readTransactionService = new ReadTransactionService(context);
                var updateTransactionService = new UpdateTransactionService(context); // Assuming this service exists
                var deleteTransactionService = new DeleteTransactionService(context);

                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("Account Transaction Management");
                    Console.WriteLine("1. Create Transaction");
                    Console.WriteLine("2. Read Transactions");
                    Console.WriteLine("3. Update Transaction");
                    Console.WriteLine("4. Delete Transaction");
                    Console.WriteLine("5. Export Transactions to Excel");
                    Console.WriteLine("0. Exit");
                    Console.Write("Select an option: ");

                    var input = Console.ReadLine();
                    switch (input)
                    {
                        case "1":
                            await CreateTransaction(createTransactionService); // Updated to use service
                            break;
                        case "2":
                            await ReadTransactions(readTransactionService); // Updated to use service
                            break;
                        case "3":
                            await UpdateTransaction(updateTransactionService); // Updated to use service
                            break;
                        case "4":
                            await DeleteTransaction(deleteTransactionService); // Updated to use service
                            break;
                        case "5":
                            var exportService = new ExportService(context);
                            await exportService.ExportToExcelAsync("ExportedData.xlsx");
                            Console.WriteLine("Data exported to Excel successfully. Press any key to continue...");
                            Console.ReadKey();
                            break;
                        case "0":
                            return;
                        default:
                            Console.WriteLine("Invalid option. Press any key to try again...");
                            Console.ReadKey();
                            break;
                    }
                }
            }
        }

        private static async Task CreateTransaction(CreateTransactionService createTransactionService)
        {
            Console.Write("Enter Account ID: ");
            var accountId = int.Parse(Console.ReadLine());

            Console.Write("Enter Transaction Description: ");
            var description = Console.ReadLine();

            Console.Write("Enter Debit or Credit (debit/credit): ");
            var debitCredit = Console.ReadLine();

            Console.Write("Enter Amount: ");
            var amount = decimal.Parse(Console.ReadLine());

            var transaction = new Transaction
            {
                Description = description,
                DebitCredit = debitCredit,
                Amount = amount,
                AccountId = accountId
            };

            try
            {
                await createTransactionService.CreateTransaction(transaction);
                Console.WriteLine("Transaction added. Press any key to continue...");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            Console.ReadKey();
        }

        private static async Task ReadTransactions(ReadTransactionService readTransactionService)
        {
            Console.Write("Enter Account ID: ");
            var accountId = int.Parse(Console.ReadLine());

            var transactions = await readTransactionService.GetTransactions(accountId);

            foreach (var transaction in transactions)
            {
                Console.WriteLine($"ID: {transaction.Id}, Description: {transaction.Description}, Type: {transaction.DebitCredit}, Amount: {transaction.Amount}");
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private static async Task UpdateTransaction(UpdateTransactionService updateTransactionService) // Assuming service implementation
        {
            Console.Write("Enter Transaction ID to update: ");
            var transactionId = int.Parse(Console.ReadLine());

            Console.Write("Enter new Description: ");
            var description = Console.ReadLine();

            Console.Write("Enter new Debit or Credit (debit/credit): ");
            var debitCredit = Console.ReadLine();

            Console.Write("Enter new Amount: ");
            var amount = decimal.Parse(Console.ReadLine());

            var transaction = new Transaction
            {
                Id = transactionId,
                Description = description,
                DebitCredit = debitCredit,
                Amount = amount
            };

            try
            {
                await updateTransactionService.UpdateTransaction(transaction);
                Console.WriteLine("Transaction updated. Press any key to continue...");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            Console.ReadKey();
        }

        private static async Task DeleteTransaction(DeleteTransactionService deleteTransactionService)
        {
            Console.Write("Enter Transaction ID to delete: ");
            var transactionId = int.Parse(Console.ReadLine());

            try
            {
                await deleteTransactionService.DeleteTransaction(transactionId);
                Console.WriteLine("Transaction deleted. Press any key to continue...");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            Console.ReadKey();
        }
    }
}
