using System.Threading.Tasks;

namespace AnvilBank.Services.Contracts
{
    public interface IUserService
    {
        Task<string> GetUserIdByUsernameAsync(string username);
        Task<string> GetAccountOwnerFullnameAsync(string userId);
    }
}
