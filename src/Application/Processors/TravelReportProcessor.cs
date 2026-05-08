using Application.Contracts;
using Application.Mappers;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Application.Processors;

public class TravelReportProcessor(IFileStorage fileStorage, IExcelService excelService, ITravelQuery travelQuery, IIdempotencyRepository idempotencyRepository, ILogger<TravelReportProcessor> logger) : IEventProcessor
{
    private readonly ILogger<TravelReportProcessor> _logger = logger;
    private readonly IFileStorage _fileStorage = fileStorage;
    private readonly IExcelService _excelService = excelService;
    private readonly ITravelQuery _travelQuery = travelQuery;
    private readonly IIdempotencyRepository _idempotencyRepository = idempotencyRepository;

    public async Task ProcessAsync(
        string message,
        CancellationToken cancellationToken)
    {
        if (message == null)
        {
            _logger.LogWarning("Mensagem nula ou invalida para processamento");
        }
        _logger.LogInformation("Processando mensagem: {message}", message);

        var messageDecoded = JsonSerializer.Deserialize<Contracts.Events.TravelReportEvent>(message);

        if (messageDecoded is null) return;

        if (await _idempotencyRepository.ThereIsAsync(messageDecoded.EventId))
        {
            _logger.LogWarning("Processamento da message já foi feito: {message}", message);
            return;
        }

        await _idempotencyRepository.SaveAsync(messageDecoded.EventId, "TravelReportEvent", message);

        var (startDate, endDate) = PreparePeriod(messageDecoded.MonthTravel, messageDecoded.YearTravel);
        var listElements = await _travelQuery.ListTravelAsync(messageDecoded.VehicleId, messageDecoded.DestinationId, startDate, endDate);

        var data = TravelMapper.ToExcelDocument(listElements.ToList());
        var file = _excelService.Generate(data);
        await _fileStorage.SaveAsync(data.Name, file);
        await _idempotencyRepository.UpdateAsync(messageDecoded.EventId, DateTime.UtcNow);

        await Task.CompletedTask;
    }

    private static (DateTime? Start, DateTime? End) PreparePeriod(int? month, int? year)
    {
        if (!year.HasValue) return (null, null);

        if (month.HasValue && (month < 1 || month > 12)) return (null, null);

        var start = new DateTime(year.Value, month ?? 1, 1);
        var end = month.HasValue
            ? start.AddMonths(1)
            : start.AddYears(1);

        return (start, end);
    }
}
