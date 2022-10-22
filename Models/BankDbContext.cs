using Microsoft.EntityFrameworkCore;

namespace DBConcurrencyManagement.Models
{
    public class BankDbContext : DbContext
    {
        public BankDbContext(DbContextOptions<BankDbContext> options) : base(options)
        {
        }
        
        public DbSet<BankAccount> BankAccounts { get; set; }
    }
}
