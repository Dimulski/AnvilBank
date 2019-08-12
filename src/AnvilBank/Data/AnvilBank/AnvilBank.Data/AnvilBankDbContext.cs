using AnvilBank.Data.Configurations;
using AnvilBank.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AnvilBank.Data
{
    public class AnvilBankDbContext : IdentityDbContext<BankUser>
    {
        public AnvilBankDbContext(DbContextOptions<AnvilBankDbContext> options)
            : base(options)
        {
        }

        public DbSet<BankAccount> Accounts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserConfiguration());

            base.OnModelCreating(builder);
        }
    }
}
