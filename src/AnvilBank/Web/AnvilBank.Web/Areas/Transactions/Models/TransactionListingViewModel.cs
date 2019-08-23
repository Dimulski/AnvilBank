using AnvilBank.Web.Infrastructure.Collections;

namespace AnvilBank.Web.Areas.Transactions.Models
{
    public class TransactionListingViewModel
    {
        public PaginatedList<TransactionListingDto> Transactions { get; set; }
    }
}
