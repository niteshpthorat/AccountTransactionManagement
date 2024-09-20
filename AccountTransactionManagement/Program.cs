using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore; 
using AccountTransactionManagement.Data;

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

            }

            
            Console.WriteLine("Application started. Press any key to exit...");
            Console.ReadKey();
        }
    }
}
