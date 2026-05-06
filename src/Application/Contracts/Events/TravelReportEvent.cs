namespace Application.Contracts.Events;

public class TravelReportEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTime OccurredAt { get; init; } = DateTime.UtcNow;
    public Guid? VehicleId { get; set; }
    public Guid? DestinationId { get; set; }
    public int? MonthTravel { get; set; }
    public int? YearTravel { get; set; }
}
