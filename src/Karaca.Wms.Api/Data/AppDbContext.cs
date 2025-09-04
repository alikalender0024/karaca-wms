using Microsoft.EntityFrameworkCore;
using Karaca.Wms.Api.Models;

namespace Karaca.Wms.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products => Set<Product>();
        public DbSet<Location> Locations => Set<Location>();    
    }
}
