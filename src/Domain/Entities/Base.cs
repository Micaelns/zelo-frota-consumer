namespace Domain.Entities;

public class Base()
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public DateTime? Deleted { get; set; } = null;
}
