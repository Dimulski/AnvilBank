using AnvilBank.Common;
using AnvilBank.Common.Configuration;
using AnvilBank.Services.Contracts;
using AnvilBank.Services.Models.BankAccount;
using AnvilBank.Web.Models.BankAccount;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnvilBank.Web.Controllers
{
    public class BankAccountsController : BaseController
    {
        private const int ItemsPerPage = 10;

        private readonly IBankAccountService bankAccountService;
        private readonly IUserService userService;
        private readonly BankConfiguration bankConfiguration;
        //private readonly ITransactionService transactionService;

        public BankAccountsController(
            IBankAccountService bankAccountService,
            IUserService userService,
            IOptions<BankConfiguration> bankConfigurationOptions)
        {
            this.bankAccountService = bankAccountService;
            this.userService = userService;
            this.bankConfiguration = bankConfigurationOptions.Value;
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(BankAccountCreateBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var serviceModel = Mapper.Map<BankAccountCreateServiceModel>(model);
            serviceModel.UserId = await this.userService.GetUserIdByUsernameAsync(this.User.Identity.Name);

            var accountId = await this.bankAccountService.CreateAsync(serviceModel);
            if (accountId == null)
            {
                this.ShowErrorMessage(NotificationMessages.BankAccountCreateError);

                return this.View(model);
            }

            this.ShowSuccessMessage(NotificationMessages.BankAccountCreated);

            return this.RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Details(string id, int pageIndex = 1)
        {
            pageIndex = Math.Max(1, pageIndex);

            var account = await this.bankAccountService.GetByIdAsync<BankAccountDetailsServiceModel>(id);
            if (account == null ||
                account.UserUsername != this.User.Identity.Name)
            {
                return this.Forbid();
            }

            //

            var viewModel = Mapper.Map<BankAccountDetailsViewModel>(account);
            //
            //
            viewModel.BankName = this.bankConfiguration.BankName;
            viewModel.BankCode = this.bankConfiguration.UniqueIdentifier;
            viewModel.BankCountry = this.bankConfiguration.Country;

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeAccountNameAsync(string accountId, string name)
        {
            if (name == null)
            {
                return this.Ok(new
                {
                    success = false
                });
            }

            var account = await this.bankAccountService.GetByIdAsync<BankAccountDetailsServiceModel>(accountId);
            if (account == null ||
                account.UserUsername != this.User.Identity.Name)
            {
                return this.Ok(new
                {
                    success = false
                });
            }

            bool isSuccessful = await this.bankAccountService.ChangeAccountNameAsync(accountId, name);

            return this.Ok(new
            {
                success = isSuccessful
            });
        }
    }
}
