using AnvilBank.Common.Utils;
using AnvilBank.Data;

namespace AnvilBank.Services.Implementations
{
    public abstract class BaseService
    {
        protected readonly AnvilBankDbContext context;

        protected BaseService(AnvilBankDbContext context)
        {
            this.context = context;
        }

        protected bool IsEntityStateValid(object model)
            => ValidationUtil.IsObjectValid(model);
    }
}
