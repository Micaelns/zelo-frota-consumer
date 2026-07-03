namespace Application.DTOs;

public class TravelDTO
{
    public Guid Id { get; set; }
    public string VehiclePlate { get; set; } = string.Empty;
    public string TravelDestination { get; set; } = string.Empty;
    public int? StartedMileage { get; set; }
    public int? FinishedMileage { get; set; }
    public float? Autonomy { get; set; }
    public DateTime? Start { get; set; }
    public DateTime? End { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Deleted { get; set; }
}
