namespace Domain.Entities;

public class Vehicle : Base
{
    public VehicleType VehicleType { get; set; }
    public string Plate { get; set; } = string.Empty;
    public int Mileage { get; private set; } = default;
    public readonly List<Travel> Travels = [];
}
