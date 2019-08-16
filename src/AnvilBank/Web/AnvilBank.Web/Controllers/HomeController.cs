using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace AnvilBank.Web.Controllers
{
    [AllowAnonymous]
    public class HomeController : BaseController
    {
        public async Task<IActionResult> Index()
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                return this.View("IndexGuest");
            }

            return this.View();
        }
    }
}
