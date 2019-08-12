using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AnvilBank.Common;
using Microsoft.AspNetCore.Identity;

namespace AnvilBank.Models
{
    public class BankUser : IdentityUser
    {
        [Required]
        [MaxLength(ModelConstants.User.FullNameMaxLength)]
        public string FullName { get; set; }

        public ICollection<BankAccount> BankAccounts { get; set; }

        //public ICollection<Card> Cards { get; set; }
    }
}
