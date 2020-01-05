using System;
using System.Threading.Tasks;
using AnvilBank.Common.Configuration;
using AnvilBank.Common.Utils;
using AnvilBank.Services.Contracts;
using AnvilBank.Services.Models.BankAccount;
using AnvilBank.Services.Models.Transaction;
using AnvilBank.Web.Infrastructure.Helpers.GlobalTransactionHelper.Models;
using Microsoft.Extensions.Options;

namespace AnvilBank.Web.Infrastructure.Helpers.GlobalTransactionHelper
{
    public class GlobalTransactionHelper : IGlobalTransactionHelper
    {
        private readonly IBankAccountService bankAccountService;
        private readonly BankConfiguration bankConfiguration;
        private readonly ITransactionService transactionService;

        public GlobalTransactionHelper(
            IBankAccountService bankAccountService,
            IOptions<BankConfiguration> bankConfigurationOptions,
            ITransactionService transactionService)
        {
            this.bankAccountService = bankAccountService;
            this.bankConfiguration = bankConfigurationOptions.Value;
            this.transactionService = transactionService;
        }

        public async Task<GlobalTransactionResult> TransactAsync(GlobalTransactionDto model)
        {
            if (!ValidationUtil.IsObjectValid(model))
            {
                return GlobalTransactionResult.GeneralFailure;
            }

            // TODO: Remove this check or the one in PaymentsController
            var account = await this.bankAccountService
                .GetByUniqueIdAsync<BankAccountConciseServiceModel>(model.DestinationBankAccountUniqueId);

            if (account == null)
            {
                return GlobalTransactionResult.GeneralFailure;
            }

            if (account.Balance < model.Amount)
            {
                return GlobalTransactionResult.InsufficientFunds;
            }

            var serviceModel = new TransactionCreateServiceModel
            {
                Amount = -model.Amount,
                Source = account.UniqueId,
                Description = model.Description ?? String.Empty,
                AccountId = account.Id,
                DestinationBankAccountUniqueId = model.DestinationBankAccountUniqueId,
                SenderName = account.UserFullName,
                RecipientName = model.RecipientName,
                ReferenceNumber = ReferenceNumberGenerator.GenerateReferenceNumber()
            };

            bool success = await this.transactionService.CreateTransactionAsync(serviceModel);
            return !success ? GlobalTransactionResult.GeneralFailure : GlobalTransactionResult.Succeeded;
        }
    }
}
