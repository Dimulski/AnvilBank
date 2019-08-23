using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AnvilBank.Services.Contracts;
using AnvilBank.Web.Models;
using AnvilBank.Services.Models.BankAccount;
using System.Linq;
using AutoMapper;
using AnvilBank.Web.Models.BankAccount;
using AnvilBank.Services.Models.Transaction;
using AnvilBank.Web.Areas.Transactions.Models;

namespace AnvilBank.Web.Controllers
{
    [AllowAnonymous]
    public class HomeController : BaseController
    {
        private readonly IBankAccountService bankAccountService;
        private readonly ITransactionService transactionService;
        private readonly IUserService userService;

        public HomeController(
            IBankAccountService bankAccountService,
            ITransactionService transactionService,
            IUserService userService)
        {
            this.bankAccountService = bankAccountService;
            this.transactionService = transactionService;
            this.userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                return this.View("IndexGuest");
            }

            var userId = await this.userService.GetUserIdByUsernameAsync(this.User.Identity.Name);
            var bankAccounts =
                (await this.bankAccountService.GetAllAccountsByUserIdAsync<BankAccountIndexServiceModel>(userId))
                .Select(Mapper.Map<BankAccountIndexViewModel>)
                .ToArray();

            var transactions = (await this.transactionService
                    .GetLast10TransactionsForUserAsync<TransactionListingServiceModel>(userId))
                .Select(Mapper.Map<TransactionListingDto>)
                .ToArray();

            var viewModel = new HomeViewModel
            {
                UserBankAccounts = bankAccounts,
                Transactions = transactions
            };

            return this.View(viewModel);
        }
    }
}
