using AnvilBank.Common;
using System.ComponentModel.DataAnnotations;

namespace AnvilBank.Web.Infrastructure.Helpers.GlobalTransactionHelper.Models
{
    public class GlobalTransactionDto
    {
        [Required]
        [Range(typeof(decimal), ModelConstants.Transaction.MinStartingPrice, ModelConstants.Transaction.MaxStartingPrice)]
        public decimal Amount { get; set; }

        [Required]
        [MaxLength(ModelConstants.User.FullNameMaxLength)]
        public string RecipientName { get; set; }

        [MaxLength(ModelConstants.Transaction.DescriptionMaxLength)]
        public string Description { get; set; }

        [Required]
        [MaxLength(ModelConstants.BankAccount.UniqueIdMaxLength)]
        public string DestinationBankAccountUniqueId { get; set; }
    }
}
