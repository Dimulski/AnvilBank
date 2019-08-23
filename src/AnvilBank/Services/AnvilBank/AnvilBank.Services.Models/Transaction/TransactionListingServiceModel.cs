using System;

namespace AnvilBank.Services.Models.Transaction
{
    public class TransactionListingServiceModel : TransactionBaseServiceModel
    {
        public string Id { get; set; }

        public string Description { get; set; }

        public decimal Amount { get; set; }

        public string SenderName { get; set; }

        public string RecipientName { get; set; }

        public DateTime MadeOn { get; set; }

        public string Source { get; set; }

        public string Destination { get; set; }

        public string ReferenceNumber { get; set; }
    }
}
