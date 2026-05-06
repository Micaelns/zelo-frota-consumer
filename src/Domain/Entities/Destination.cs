namespace Domain.Entities;

public class Destination() : Base
{
    public string ZipCode { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Neighborhood { get; set; } = string.Empty;
    public string Locality { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Uf { get; set; } = string.Empty;
}
