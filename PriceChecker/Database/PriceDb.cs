using Microsoft.EntityFrameworkCore;

namespace PriceChecker.Database;

public class PriceDb : DbContext
{
    public PriceDb(DbContextOptions<PriceDb> options): base(options)
    { }

    public DbSet<Price> Prices => Set<Price>();
}