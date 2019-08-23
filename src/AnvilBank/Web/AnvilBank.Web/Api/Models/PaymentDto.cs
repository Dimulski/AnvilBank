using AnvilBank.Common;
using System.ComponentModel.DataAnnotations;

namespace AnvilBank.Web.Api.Models
{
    public class PaymentDto
    {
        [Required]
        public decimal Amount { get; set; }

        public string Description { get; set; }

        [Required]
        public string RecipientName { get; set; }

        [Required]
        [MaxLength(ModelConstants.BankAccount.UniqueIdMaxLength)]
        public string DestinationBankAccountUniqueId { get; set; }
    }
}
