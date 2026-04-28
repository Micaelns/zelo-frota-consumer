namespace Application.Contracts;

public class TravelReportEvent
{
    public Guid EventId { get; set; }
    public DateTime OccurredAt { get; set; }
    public Guid? VehicleId { get; set; }
    public Guid? DestinationId { get; set; }
    public DateTime? MonthYearTravel { get; set; }
}
