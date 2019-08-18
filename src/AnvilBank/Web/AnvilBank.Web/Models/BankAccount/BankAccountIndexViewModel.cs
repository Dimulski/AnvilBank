using AnvilBank.Common.AutoMapping.Contracts;
using AnvilBank.Services.Models.BankAccount;

namespace AnvilBank.Web.Models.BankAccount
{
    public class BankAccountIndexViewModel : IMapWith<BankAccountIndexServiceModel>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public decimal Balance { get; set; }

        public string UniqueId { get; set; }
    }
}
