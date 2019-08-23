using AnvilBank.Services.Contracts;
using AnvilBank.Services.Models.BankAccount;
using AnvilBank.Web.Controllers;
using AnvilBank.Web.Models.BankAccount;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace AnvilBank.Web.Areas.Transactions.Controllers
{
    [Area("Transactions")]
    public abstract class BaseTransactionController : BaseController
    {
        private readonly IBankAccountService bankAccountService;

        public BaseTransactionController(IBankAccountService bankAccountService)
        {
            this.bankAccountService = bankAccountService;
        }

        protected async Task<OwnBankAccountListingViewModel[]> GetAllAccountsAsync(string userId)
        {
            return (await this.bankAccountService
                    .GetAllAccountsByUserIdAsync<BankAccountIndexServiceModel>(userId))
                .Select(Mapper.Map<OwnBankAccountListingViewModel>)
                .ToArray();
        }
    }
}
