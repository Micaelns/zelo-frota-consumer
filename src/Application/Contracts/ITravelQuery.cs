using Application.DTOs;

namespace Application.Contracts;

public interface ITravelQuery
{
    public Task<IEnumerable<TravelDTO>> ListTravelAsync(Guid? vehicleId, Guid? destinationId, DateTime? startTravel, DateTime? endTravel);
}
