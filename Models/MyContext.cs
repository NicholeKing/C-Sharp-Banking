using Microsoft.EntityFrameworkCore;
 
namespace Banking.Models
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions options) : base(options) { }

        public DbSet<Account> Accounts {get;set;}
        public DbSet<Log> Logins {get;set;}
        public DbSet<Transaction> Trans {get;set;}
    }
}