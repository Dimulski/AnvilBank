using AnvilBank.Common;
using AnvilBank.Common.AutoMapping.Contracts;
using AnvilBank.Services.Contracts;
using AnvilBank.Services.Models.Transaction;
using AnvilBank.Web.Models.BankAccount;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;

namespace AnvilBank.Web.Areas.Transactions.Models.Internal
{
    public class InternalTransactionCreateBindingModel : IMapWith<TransactionCreateServiceModel>, IValidatableObject
    {
        private const string DestinationAccountIncorrectError =
            "Destination account is incorrect or belongs to a different bank.";

        [MaxLength(ModelConstants.Transaction.DescriptionMaxLength)]
        public string Description { get; set; }

        [Required]
        [Range(typeof(decimal), ModelConstants.Transaction.MinStartingPrice,
            ModelConstants.Transaction.MaxStartingPrice, ErrorMessage =
                NotificationMessages.InvalidTransactionAmount)]
        public decimal Amount { get; set; }

        [Required]
        [Display(Name = "Destination account")]
        [RegularExpression(@"^[A-Z]{4}\d{8}$", ErrorMessage = DestinationAccountIncorrectError)]
        public string DestinationBankAccountUniqueId { get; set; }

        public IEnumerable<OwnBankAccountListingViewModel> OwnAccounts { get; set; }

        [Required]
        [Display(Name = "Source account")]
        public string AccountId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var uniqueIdHelper = validationContext.GetService<IBankAccountUniqueIdHelper>();
            if (!uniqueIdHelper.IsUniqueIdValid(this.DestinationBankAccountUniqueId))
            {
                yield return new ValidationResult(DestinationAccountIncorrectError,
                    new[] { nameof(this.DestinationBankAccountUniqueId) });
            }
        }
    }
}
