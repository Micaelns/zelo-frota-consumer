using Application.Contracts;
using Microsoft.Extensions.Options;
using WorkerService.Configurations;

namespace WorkerService;

public class IdempotencyCleanupWorker(
    IOptions<CleanupSettings> cleanupSettings,
    IServiceProvider serviceProvider,
    ILogger<IdempotencyCleanupWorker> logger)
    : BackgroundService
{
    private readonly int _retencionMessagesInDays = cleanupSettings.Value.RetentionMessagesInDays;
    private readonly int _intervalHours = cleanupSettings.Value.IntervalHours;
    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        logger.LogInformation("Inicio verificação de expiração de Idempotências.");
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = serviceProvider.CreateScope();

                var repository =
                    scope.ServiceProvider
                        .GetRequiredService<IIdempotencyRepository>();

                var qtdDeleted = await repository.DeleteAllExpiredRetentionAsync(_retencionMessagesInDays);

                if (qtdDeleted > 0)
                {
                    logger.LogInformation("Removida(s) {quantidade} Idempotência(s) expirada(s).", qtdDeleted);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Erro ao remover idempotências expiradas.");
            }

            await Task.Delay(
                TimeSpan.FromHours(_intervalHours),
                stoppingToken);
        }
    }
}
