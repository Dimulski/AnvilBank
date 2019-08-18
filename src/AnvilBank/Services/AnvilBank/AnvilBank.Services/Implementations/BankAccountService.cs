using AnvilBank.Data;
using AnvilBank.Models;
using AnvilBank.Services.Contracts;
using AnvilBank.Services.Models.BankAccount;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnvilBank.Services.Implementations
{
    class BankAccountService : BaseService, IBankAccountService
    {
        private readonly IBankAccountUniqueIdHelper uniqueIdHelper;

        public BankAccountService(AnvilBankDbContext context, IBankAccountUniqueIdHelper uniqueIdHelper)
            : base(context)
        {
            this.uniqueIdHelper = uniqueIdHelper;
        }

        public async Task<string> CreateAsync(BankAccountCreateServiceModel model)
        {
            if (!this.IsEntityStateValid(model) ||
                !this.context.Users.Any(u => u.Id == model.UserId))
            {
                return null;
            }

            string generatedUniqueId;
            do
            {
                generatedUniqueId = this.uniqueIdHelper.GenerateAccountUniqueId();
            } while (this.context.Accounts.Any(a => a.UniqueId == generatedUniqueId));

            if (model.Name == null)
            {
                model.Name = generatedUniqueId;
            }

            var dbModel = Mapper.Map<BankAccount>(model);
            dbModel.UniqueId = generatedUniqueId;

            await this.context.Accounts.AddAsync(dbModel);
            await this.context.SaveChangesAsync();

            return dbModel.Id;
        }

        public async Task<T> GetByIdAsync<T>(string id)
            where T : BankAccountBaseServiceModel
            => await this.context
                .Accounts
                .Where(a => a.Id == id)
                .ProjectTo<T>()
                .SingleOrDefaultAsync();

        public async Task<T> GetByUniqueIdAsync<T>(string uniqueId)
            where T : BankAccountBaseServiceModel
            => await this.context
                .Accounts
                .Where(a => a.UniqueId == uniqueId)
                .ProjectTo<T>()
                .SingleOrDefaultAsync();

        public async Task<IEnumerable<T>> GetAccountsAsync<T>(int pageIndex = 1, int count = int.MaxValue)
            where T : BankAccountBaseServiceModel
            => await this.context
                .Accounts
                .Skip((pageIndex - 1) * count)
                .Take(count)
                .ProjectTo<T>()
                .ToArrayAsync();

        public async Task<IEnumerable<T>> GetAllAccountsByUserIdAsync<T>(string userId)
            where T : BankAccountBaseServiceModel
            => await this.context
                .Accounts
                .Where(a => a.UserId == userId)
                .ProjectTo<T>()
                .ToArrayAsync();

        public async Task<bool> ChangeAccountNameAsync(string accountId, string newName)
        {
            var account = await this.context
                            .Accounts
                            .FindAsync(accountId);
            if (account == null)
            {
                return false;
            }

            account.Name = newName;
            this.context.Update(account);
            await this.context.SaveChangesAsync();

            return true;
        }

        public async Task<int> GetCountOfAccountsAsync()
            => await this.context
                .Accounts
                .CountAsync();
    }
}
