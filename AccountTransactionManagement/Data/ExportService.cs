using OfficeOpenXml;
using System.IO;
using System.Threading.Tasks;
using AccountTransactionManagement.Models;
using Microsoft.EntityFrameworkCore;


namespace AccountTransactionManagement.Data
{
    public class ExportService
    {
        private readonly AppDbContext _context;

        public ExportService(AppDbContext context)
        {
            _context = context;
        }

        public async Task ExportToExcelAsync(string filePath)
        {
            var accounts = await _context.Accounts.ToListAsync();
            var transactions = await _context.Transactions.ToListAsync();

            using (var package = new ExcelPackage())
            {
                // Create Account sheet
                var accountSheet = package.Workbook.Worksheets.Add("Accounts");
                accountSheet.Cells[1, 1].Value = "ID";
                accountSheet.Cells[1, 2].Value = "Name";
                accountSheet.Cells[1, 3].Value = "Number";
                accountSheet.Cells[1, 4].Value = "Current Balance";
                accountSheet.Cells[1, 5].Value = "Overdraft Limit";

                for (int i = 0; i < accounts.Count; i++)
                {
                    var account = accounts[i];
                    accountSheet.Cells[i + 2, 1].Value = account.Id;
                    accountSheet.Cells[i + 2, 2].Value = account.Name;
                    accountSheet.Cells[i + 2, 3].Value = account.Number;
                    accountSheet.Cells[i + 2, 4].Value = account.CurrentBalance;
                    accountSheet.Cells[i + 2, 5].Value = account.OverdraftLimit;
                }

                // Create Transaction sheet
                var transactionSheet = package.Workbook.Worksheets.Add("Transactions");
                transactionSheet.Cells[1, 1].Value = "ID";
                transactionSheet.Cells[1, 2].Value = "Description";
                transactionSheet.Cells[1, 3].Value = "Debit/Credit";
                transactionSheet.Cells[1, 4].Value = "Amount";
                transactionSheet.Cells[1, 5].Value = "Account ID";

                for (int i = 0; i < transactions.Count; i++)
                {
                    var transaction = transactions[i];
                    transactionSheet.Cells[i + 2, 1].Value = transaction.Id;
                    transactionSheet.Cells[i + 2, 2].Value = transaction.Description;
                    transactionSheet.Cells[i + 2, 3].Value = transaction.DebitCredit;
                    transactionSheet.Cells[i + 2, 4].Value = transaction.Amount;
                    transactionSheet.Cells[i + 2, 5].Value = transaction.AccountId;
                }

                // Save the package to the specified file path
                FileInfo fi = new FileInfo(filePath);
                package.SaveAs(fi);
            }
        }
    }
}
