namespace Application.Contracts;

public interface IIdempotencyRepository
{
    public Task<bool> ThereIsAsync(Guid EventId);
    public Task SaveAsync(Guid eventId, string messageType, string payload);
    public Task UpdateAsync(Guid eventId, DateTime processEndAt);
    public Task DeleteAsync(Guid eventId);
    public Task<int> DeleteAllExpiredRetentionAsync(int days);
}
