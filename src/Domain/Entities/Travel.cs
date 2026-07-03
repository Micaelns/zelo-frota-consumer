namespace Domain.Entities;

public class Travel : Base
{
    public required Vehicle Vehicle { get; set; } 
    public required Destination Destination { get; set; }
    public Guid VehicleId { get; set; }
    public Guid DestinationId { get; set; }
    public int? StartedMileage { get; set; }
    public int? FinishedMileage { get; set; }
    public float? Autonomy { get; set; }
    public DateTime? Start { get; set; }
    public DateTime? End { get; set; }
}
