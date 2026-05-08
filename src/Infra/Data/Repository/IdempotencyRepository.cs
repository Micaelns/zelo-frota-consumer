using Application.Contracts;
using Infra.Data.Contexts;
using Infra.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Repository;

public class IdempotencyRepository(ZeloFrotaDbContext context) : IIdempotencyRepository
{
    private readonly ZeloFrotaDbContext _context = context;

    public async Task<bool> ThereIsAsync(Guid eventId)
    {
        return await _context.Idempodencies.AnyAsync(element => element.EventId == eventId);
    }

    public async Task SaveAsync(Guid eventId, string messageType, string payload)
    {
        var idempotency = new Idempodency { EventId = eventId, MessageType = messageType, Payload = payload };
        await _context.Idempodencies.AddAsync(idempotency);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Guid eventId, DateTime processEndAt)
    {
        await _context.Idempodencies
                    .Where(element => element.EventId == eventId)
                    .ExecuteUpdateAsync(s => s.SetProperty(
                                            x => x.ProcessEndAt,
                                            processEndAt));
    }

    public async Task DeleteAsync(Guid eventId)
    {
        await _context.Idempodencies
                    .Where(element => element.EventId == eventId)
                    .ExecuteDeleteAsync();
    }

    public async Task<int> DeleteAllExpiredRetentionAsync(int days)
    {
        return await _context.Idempodencies
                    .Where(element => element.ProcessEndAt < DateTime.UtcNow.AddDays(-days))
                    .ExecuteDeleteAsync();
    }
}
