using System.ComponentModel.DataAnnotations;

namespace DBConcurrencyManagement.Models
{
    public class BankAccount
    {
        [Key]
        public long Id { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public decimal Balance { get; set; }
        public BankAccount(string accountNumber, string accountName, decimal balance)
        {
            AccountNumber = accountNumber;
            AccountName = accountName;
            Balance = balance;
        }
    }
}
