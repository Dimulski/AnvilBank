using AnvilBank.Common.AutoMapping.Contracts;
using AnvilBank.Services.Models.BankAccount;
using System;

namespace AnvilBank.Web.Models.BankAccount
{
    public class BankAccountDetailsViewModel : IMapWith<BankAccountDetailsServiceModel>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public decimal Balance { get; set; }

        public string UniqueId { get; set; }

        public DateTime CreatedOn { get; set; }

        public string UserFullName { get; set; }

        //public PaginatedList<TransactionListingDto> Transactions { get; set; }

        //public int TransactionsCount { get; set; }

        public string BankName { get; set; }

        public string BankCode { get; set; }

        public string BankCountry { get; set; }
    }
}
