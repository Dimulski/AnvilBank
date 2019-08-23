using Microsoft.EntityFrameworkCore;

namespace AnvilApi.Data
{
    public class AnvilApiDbContext : DbContext
    {
        public AnvilApiDbContext(DbContextOptions<AnvilApiDbContext> options)
            : base(options)
        {
        }
    }
}
