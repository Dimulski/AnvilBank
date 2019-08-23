using AnvilBank.Common.AutoMapping.Contracts;
using AnvilBank.Web.Infrastructure.Helpers.GlobalTransactionHelper.Models;
using System.ComponentModel.DataAnnotations;

namespace AnvilBank.Web.Api.Models
{
    public class PaymentInfoModel : IMapWith<GlobalTransactionDto>
    {
        [Required]
        public decimal Amount { get; set; }

        public string Description { get; set; }

        [Required]
        public string DestinationBankAccountUniqueId { get; set; }
    }
}
