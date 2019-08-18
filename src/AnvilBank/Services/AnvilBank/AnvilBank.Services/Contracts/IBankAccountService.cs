using AnvilBank.Services.Models.BankAccount;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnvilBank.Services.Contracts
{
    public interface IBankAccountService
    {
        Task<IEnumerable<T>> GetAllAccountsByUserIdAsync<T>(string userId)
            where T : BankAccountBaseServiceModel;

        Task<string> CreateAsync(BankAccountCreateServiceModel model);

        Task<T> GetByUniqueIdAsync<T>(string uniqueId)
            where T : BankAccountBaseServiceModel;

        Task<T> GetByIdAsync<T>(string id)
            where T : BankAccountBaseServiceModel;

        Task<bool> ChangeAccountNameAsync(string accountId, string newName);

        Task<IEnumerable<T>> GetAccountsAsync<T>(int pageIndex = 1, int count = int.MaxValue)
            where T : BankAccountBaseServiceModel;

        Task<int> GetCountOfAccountsAsync();
    }
}
