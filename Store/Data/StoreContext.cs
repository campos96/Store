using Microsoft.EntityFrameworkCore;
using Store.Models;

namespace Store.Data
{
    public class StoreContext : DbContext
    {
        public StoreContext (DbContextOptions<StoreContext> options) : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; } = default!;
        public DbSet<User> User { get; set; } = default!;
        public DbSet<Product> Products { get; set; } = default!;
        public DbSet<ProductCategory> ProductCategories { get; set; } = default!;
        public DbSet<Inventory> Inventory { get; set; } = default!;
    }
}
