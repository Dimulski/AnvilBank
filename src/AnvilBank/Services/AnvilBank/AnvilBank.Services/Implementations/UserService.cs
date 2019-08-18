using AnvilBank.Data;
using AnvilBank.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AnvilBank.Services.Implementations
{
    public class UserService : BaseService, IUserService
    {
        public UserService(AnvilBankDbContext context)
            : base(context)
        {
        }

        public async Task<string> GetUserIdByUsernameAsync(string username)
        {
            var user = await this.context
                .Users
                .SingleOrDefaultAsync(u => u.UserName == username);

            return user?.Id;
        }

        public async Task<string> GetAccountOwnerFullnameAsync(string userId)
        {
            var user = await this.context
                .Users
                .SingleOrDefaultAsync(u => u.Id == userId);

            return user?.FullName;
        }
    }
}
