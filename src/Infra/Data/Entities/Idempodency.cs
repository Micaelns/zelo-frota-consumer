namespace Infra.Data.Entities;

public class Idempodency
{
    public Guid EventId { get; set; }
    public string MessageType { get; set; } = string.Empty;
    public string Payload { get; set; } = string.Empty;
    public DateTime ProcessStartAt { get; set; }
    public DateTime? ProcessEndAt { get; set; }
}
