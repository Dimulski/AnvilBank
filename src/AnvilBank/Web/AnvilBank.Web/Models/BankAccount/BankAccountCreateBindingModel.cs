using AnvilBank.Common.AutoMapping.Contracts;
using AnvilBank.Services.Models.BankAccount;
using System.ComponentModel.DataAnnotations;

namespace AnvilBank.Web.Models.BankAccount
{
    public class BankAccountCreateBindingModel : IMapWith<BankAccountCreateServiceModel>
    {
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
