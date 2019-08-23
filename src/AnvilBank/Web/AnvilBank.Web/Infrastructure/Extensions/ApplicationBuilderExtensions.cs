using AnvilBank.Common;
using AnvilBank.Data;
using AnvilBank.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnvilBank.Web.Infrastructure.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder InitializeDatabase(this IApplicationBuilder app)
        {
            InitializeDatabaseAsync(app).GetAwaiter().GetResult();

            return app;
        }

        private static async Task InitializeDatabaseAsync(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<AnvilBankDbContext>();
                var userManager = serviceScope.ServiceProvider.GetService<UserManager<BankUser>>();
                var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();

                await dbContext.Database.MigrateAsync();
                await SeedUser(userManager, dbContext);

                if (!await roleManager.RoleExistsAsync(GlobalConstants.AdministratorRoleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(GlobalConstants.AdministratorRoleName));
                }
            }
        }

        private static async Task SeedUser(UserManager<BankUser> userManager, AnvilBankDbContext dbContext)
        {
            if (!dbContext.Users.Any())
            {
                var user = new BankUser
                {
                    Email = "pesho@anvilbank.com",
                    FullName = "Pesho Peshev",
                    UserName = "pesho@anvilbank.com",
                    EmailConfirmed = true,
                    BankAccounts = new List<BankAccount>
                    {
                        new BankAccount
                        {
                            Balance = 10000,
                            CreatedOn = DateTime.UtcNow,
                            Name = "Main account",
                            UniqueId = "AVLJ98131785"
                        }
                    }
                };

                await userManager.CreateAsync(user, "Test123$");
            }
        }

        //public static IApplicationBuilder AddDefaultSecurityHeaders(
        //    this IApplicationBuilder app,
        //    SecurityHeadersBuilder builder)
        //    => app.UseMiddleware<SecurityHeadersMiddleware>(builder.Policy());
    }
}