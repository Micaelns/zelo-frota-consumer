using Domain.Entities;
using Infra.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infra.Data.Contexts;

public class ZeloFrotaDbContext(DbContextOptions<ZeloFrotaDbContext> options) : DbContext(options)
{
    public DbSet<Destination> Destinations => Set<Destination>();
    public DbSet<VehicleType> VehicleTypes => Set<VehicleType>();
    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<Travel> Travels => Set<Travel>();
    public DbSet<Idempodency> Idempodencies => Set<Idempodency>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Idempodency>(entity =>
        {
            entity.HasKey(v => v.EventId);
            entity.Property(x => x.ProcessStartAt)
                    .HasDefaultValueSql("SYSUTCDATETIME()");
        });
    }
}