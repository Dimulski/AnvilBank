using AnvilBank.Common;
using AnvilBank.Services.Contracts;
using AnvilBank.Services.Models.BankAccount;
using AnvilBank.Services.Models.Transaction;
using AnvilBank.Web.Areas.Transactions.Models.Internal;
using AnvilBank.Web.Infrastructure;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnvilBank.Web.Areas.Transactions.Controllers
{
    public class InternalController : BaseTransactionController
    {
        private readonly IBankAccountService bankAccountService;
        private readonly ITransactionService transactionService;
        private readonly IUserService userService;

        public InternalController(
            ITransactionService transactionService,
            IBankAccountService bankAccountService,
            IUserService userService)
            : base(bankAccountService)
        {
            this.transactionService = transactionService;
            this.userService = userService;
            this.bankAccountService = bankAccountService;
        }

        [Route("/{area}/Create")]
        public async Task<IActionResult> Create()
        {
            var userId = await this.userService.GetUserIdByUsernameAsync(this.User.Identity.Name);
            var userAccounts = await this.GetAllAccountsAsync(userId);

            if (!userAccounts.Any())
            {
                this.ShowErrorMessage(NotificationMessages.NoAccountsError);

                return this.RedirectToHome();
            }

            var model = new InternalTransactionCreateBindingModel
            {
                OwnAccounts = userAccounts
            };

            return this.View(model);
        }

        [Route("/{area}/Create")]
        [HttpPost]
        public async Task<IActionResult> Create(InternalTransactionCreateBindingModel model)
        {
            var userId = await this.userService.GetUserIdByUsernameAsync(this.User.Identity.Name);

            if (!this.ModelState.IsValid)
            {
                model.OwnAccounts = await this.GetAllAccountsAsync(userId);

                return this.View(model);
            }

            var account = await this.bankAccountService.GetByIdAsync<BankAccountDetailsServiceModel>(model.AccountId);
            if (account == null || account.UserUsername != this.User.Identity.Name)
            {
                return this.Forbid();
            }

            if (string.Equals(account.UniqueId, model.DestinationBankAccountUniqueId,
                StringComparison.InvariantCulture))
            {
                this.ShowErrorMessage(NotificationMessages.SameAccountsError);
                model.OwnAccounts = await this.GetAllAccountsAsync(userId);

                return this.View(model);
            }

            if (account.Balance < model.Amount)
            {
                this.ShowErrorMessage(NotificationMessages.InsufficientFunds);
                model.OwnAccounts = await this.GetAllAccountsAsync(userId);

                return this.View(model);
            }

            var destinationAccount =
                await this.bankAccountService.GetByUniqueIdAsync<BankAccountConciseServiceModel>(
                    model.DestinationBankAccountUniqueId);
            if (destinationAccount == null)
            {
                this.ShowErrorMessage(NotificationMessages.DestinationBankAccountDoesNotExist);
                model.OwnAccounts = await this.GetAllAccountsAsync(userId);

                return this.View(model);
            }

            var referenceNumber = ReferenceNumberGenerator.GenerateReferenceNumber();
            var sourceServiceModel = Mapper.Map<TransactionCreateServiceModel>(model);
            sourceServiceModel.Source = account.UniqueId;
            sourceServiceModel.Amount *= -1;
            sourceServiceModel.SenderName = account.UserFullName;
            sourceServiceModel.RecipientName = destinationAccount.UserFullName;
            sourceServiceModel.ReferenceNumber = referenceNumber;

            if (!await this.transactionService.CreateTransactionAsync(sourceServiceModel))
            {
                this.ShowErrorMessage(NotificationMessages.TryAgainLaterError);
                model.OwnAccounts = await this.GetAllAccountsAsync(userId);

                return this.View(model);
            }

            var destinationServiceModel = Mapper.Map<TransactionCreateServiceModel>(model);
            destinationServiceModel.Source = account.UniqueId;
            destinationServiceModel.AccountId = destinationAccount.Id;
            destinationServiceModel.SenderName = account.UserFullName;
            destinationServiceModel.RecipientName = destinationAccount.UserFullName;
            destinationServiceModel.ReferenceNumber = referenceNumber;

            if (!await this.transactionService.CreateTransactionAsync(destinationServiceModel))
            {
                this.ShowErrorMessage(NotificationMessages.TryAgainLaterError);
                model.OwnAccounts = await this.GetAllAccountsAsync(userId);

                return this.View(model);
            }

            this.ShowSuccessMessage(NotificationMessages.SuccessfulTransaction);

            return this.RedirectToHome();
        }
    }
}