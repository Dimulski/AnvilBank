using AnvilBank.Services.Contracts;
using AnvilBank.Services.Models.BankAccount;
using AnvilBank.Web.Api.Models;
using AnvilBank.Web.Infrastructure.Helpers.GlobalTransactionHelper;
using AnvilBank.Web.Infrastructure.Helpers.GlobalTransactionHelper.Models;
using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AnvilBank.Web.Api
{
    [EnableCors("AllowOrigin")]
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IBankAccountService bankAccountService;
        private readonly IGlobalTransactionHelper globalTransactionHelper;

        public PaymentsController(IBankAccountService bankAccountService, IGlobalTransactionHelper globalTransactionHelper)
        {
            this.bankAccountService = bankAccountService;
            this.globalTransactionHelper = globalTransactionHelper;
        }

        // POST: api/Payments
        [EnableCors("AllowOrigin")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PaymentDto data)
        {
            // Angular-Store should send a serialized string object for better security
            // var model = JsonConvert.DeserializeObject<PaymentInfoModel>(data);

            if (data.Amount <= 0)
            {
                return this.BadRequest();
            }

            var bankAccount = await this.bankAccountService.GetByUniqueIdAsync<BankAccountConciseServiceModel>
                (data.DestinationBankAccountUniqueId);

            if (bankAccount == null)
            {
                return this.BadRequest();
            }

            var serviceModel = Mapper.Map<GlobalTransactionDto>(data);
            var result = await this.globalTransactionHelper.TransactAsync(serviceModel);

            if (result != GlobalTransactionResult.Succeeded)
            {
                return this.BadRequest();
            }

            return this.Ok();
        }
    }
}
