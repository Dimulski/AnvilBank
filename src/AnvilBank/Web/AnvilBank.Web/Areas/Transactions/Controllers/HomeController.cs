using AnvilBank.Services.Contracts;
using AnvilBank.Services.Models.Transaction;
using AnvilBank.Web.Areas.Transactions.Models;
using AnvilBank.Web.Infrastructure.Collections;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AnvilBank.Web.Areas.Transactions.Controllers
{
    public class HomeController : BaseTransactionController
    {
        private const int PaymentsCountPerPage = 10;

        private readonly ITransactionService transactionService;
        private readonly IUserService userService;

        public HomeController(
            IBankAccountService bankAccountService,
            ITransactionService transactionService,
            IUserService userService)
            : base(bankAccountService)
        {
            this.transactionService = transactionService;
            this.userService = userService;
        }

        [Route("/{area}/Archives")]
        public async Task<IActionResult> All(int pageIndex = 1)
        {
            pageIndex = Math.Max(1, pageIndex);

            var userId = await this.userService.GetUserIdByUsernameAsync(this.User.Identity.Name);
            var allTransactions =
                (await this.transactionService.GetTransactionsAsync<TransactionListingServiceModel>(userId, pageIndex, PaymentsCountPerPage))
                .Select(Mapper.Map<TransactionListingDto>)
                .ToPaginatedList(await this.transactionService.GetCountOfAllTransactionsForUserAsync(userId), pageIndex, PaymentsCountPerPage);

            var transactions = new TransactionListingViewModel
            {
                Transactions = allTransactions
            };

            return this.View(transactions);
        }
    }
}
