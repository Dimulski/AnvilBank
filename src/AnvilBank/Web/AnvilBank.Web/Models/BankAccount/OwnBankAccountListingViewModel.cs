using AnvilBank.Common.AutoMapping.Contracts;
using AnvilBank.Services.Models.BankAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnvilBank.Web.Models.BankAccount
{
    public class OwnBankAccountListingViewModel : IMapWith<BankAccountIndexServiceModel>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public decimal Balance { get; set; }

        public string UniqueId { get; set; }
    }
}
