using AnvilBank.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AnvilBank.Models
{
    public class BankAccount
    {
        public string Id { get; set; }

        [Required]
        [MaxLength(ModelConstants.BankAccount.NameMaxLength)]
        public string Name { get; set; }

        [Required]
        public decimal Balance { get; set; }

        [Required]
        [MaxLength(ModelConstants.BankAccount.UniqueIdMaxLength)]
        public string UniqueId { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        public string UserId { get; set; }

        public BankUser User { get; set; }

        //public ICollection<Card> Cards { get; set; }

        //public ICollection<Transaction> Transactions { get; set; }
    }
}
