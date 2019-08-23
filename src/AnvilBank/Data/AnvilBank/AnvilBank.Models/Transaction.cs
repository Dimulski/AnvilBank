using AnvilBank.Common;
using System;
using System.ComponentModel.DataAnnotations;

namespace AnvilBank.Models
{
    public class Transaction
    {
        public string Id { get; set; }

        [MaxLength(ModelConstants.Transaction.DescriptionMaxLength)]
        public string Description { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public DateTime MadeOn { get; set; }

        [Required]
        public string AccountId { get; set; }

        public BankAccount Account { get; set; }

        [Required]
        [MaxLength(ModelConstants.BankAccount.UniqueIdMaxLength)]
        public string Source { get; set; }

        [Required]
        [MaxLength(ModelConstants.User.FullNameMaxLength)]
        public string SenderName { get; set; }

        [Required]
        [MaxLength(ModelConstants.User.FullNameMaxLength)]
        public string RecipientName { get; set; }

        [Required]
        [MaxLength(ModelConstants.BankAccount.UniqueIdMaxLength)]
        public string Destination { get; set; }

        [Required]
        public string ReferenceNumber { get; set; }
    }
}
