using AnvilBank.Web.Infrastructure.Attributes;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AnvilBank.Web.Models
{
    public abstract class BaseReCaptchaModel
    {
        [Required]
        [ValidateReCaptcha]
        [BindProperty(Name = "g-recaptcha-response")]
        public string ReCaptchaResponse { get; set; }
    }
}
