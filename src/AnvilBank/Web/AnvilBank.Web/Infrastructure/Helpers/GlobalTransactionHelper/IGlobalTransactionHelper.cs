using AnvilBank.Web.Infrastructure.Helpers.GlobalTransactionHelper.Models;
using System.Threading.Tasks;

namespace AnvilBank.Web.Infrastructure.Helpers.GlobalTransactionHelper
{

    public interface IGlobalTransactionHelper
    {
        Task<GlobalTransactionResult> TransactAsync(GlobalTransactionDto model);
    }
}