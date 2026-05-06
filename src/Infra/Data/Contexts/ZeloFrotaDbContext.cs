using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Contexts;

public class ZeloFrotaDbContext(DbContextOptions<ZeloFrotaDbContext> options) : DbContext(options)
{
    public DbSet<Destination> Destinations => Set<Destination>();
    public DbSet<VehicleType> VehicleTypes => Set<VehicleType>();
    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<Travel> Travels => Set<Travel>();
}
