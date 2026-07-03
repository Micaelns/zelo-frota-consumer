using Application.Contracts;
using Application.DTOs;
using Infra.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Queries;

public class TravelQuery(ZeloFrotaDbContext context) : ITravelQuery
{
    private readonly ZeloFrotaDbContext _context = context;

    public async Task<IEnumerable<TravelDTO>> ListTravelAsync(Guid? vehicleId, Guid? destinationId, DateTime? startTravel, DateTime? endTravel)
    {
        return await _context.Travels
             .AsNoTracking()
             .Where(t => vehicleId == null || t.VehicleId == vehicleId)
             .Where(t => destinationId == null || t.DestinationId == destinationId)
             .Where(t => startTravel == null || t.Start >= startTravel)
             .Where(t => endTravel == null || t.Start <= endTravel)
             .OrderByDescending(element => element.Start)
             .Select(t => new TravelDTO
             {
                 Id = t.Id,
                 VehiclePlate = t.Vehicle.Plate,
                 TravelDestination = t.Destination.Uf,
                 StartedMileage = t.StartedMileage,
                 FinishedMileage = t.FinishedMileage,
                 Autonomy = t.Autonomy,
                 Start = t.Start,
                 End = t.End,
                 Created = t.Created,
                 Deleted = t.Deleted
             })
             .ToListAsync();
    }
}
