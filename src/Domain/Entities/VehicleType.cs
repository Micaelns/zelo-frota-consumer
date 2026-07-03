namespace Domain.Entities;

public class VehicleType(string name) : Base()
{
    public string Name { get; set; } = name;
}
