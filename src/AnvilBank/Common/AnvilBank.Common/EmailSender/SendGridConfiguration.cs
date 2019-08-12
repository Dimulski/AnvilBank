using System.ComponentModel.DataAnnotations;

namespace AnvilBank.Common.EmailSender
{
    public class SendGridConfiguration
    {
        [Required]
        public string ApiKey { get; set; }
    }
}
