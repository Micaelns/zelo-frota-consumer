using Microsoft.Extensions.Logging;

namespace Application.Processors;

public class TravelReportProcessor(ILogger<TravelReportProcessor> logger) : IEventProcessor
{
    private readonly ILogger _logger = logger;

    public async Task ProcessAsync(
        string message,
        CancellationToken cancellationToken)
    {
        if (message == null)
        {
            _logger.LogWarning("Mensagem nula ou invalida para processamento");
        }
        _logger.LogInformation("Processando mensagem: {@message}", message);

        await Task.CompletedTask;
    }
}
