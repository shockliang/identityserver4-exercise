using Microsoft.EntityFrameworkCore;

namespace Bank.Api.Models
{
    public class BankContext : DbContext
    {
        public BankContext(DbContextOptions<BankContext> options)
            : base(options)
        {
        }
        
        public DbSet<Customer> Customers { get; set; }
    }
}