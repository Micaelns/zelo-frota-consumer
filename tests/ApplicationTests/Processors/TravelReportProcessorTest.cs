using Application.Contracts;
using Application.Contracts.Events;
using Application.Processors;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Moq;
using System.Diagnostics.Contracts;
using System.Text.Json;

namespace ApplicationTests.Processors;

public class TravelReportProcessorTest
{
    private readonly CancellationToken _cancellationToken = new();
    private readonly Mock<ILogger<TravelReportProcessor>> _logger;
    private readonly Mock<IFileStorage> _fileStorage;
    private readonly Mock<IExcelService> _excelService;
    private readonly Mock<ITravelQuery> _travelQuery;
    private readonly Mock<IIdempotencyRepository> _idempotencyRepository;

    public TravelReportProcessorTest()
    {
        _logger = new Mock<ILogger<TravelReportProcessor>>();
        _fileStorage = new Mock<IFileStorage>();
        _excelService = new Mock<IExcelService>();
        _travelQuery = new Mock<ITravelQuery>();
        _idempotencyRepository = new Mock<IIdempotencyRepository>();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    [InlineData("null")]
    public void ProcessAsync_InvalidMessage_ReturnWarnningMessageNula(string message)
    {
        var messageWarning = "Mensagem nula";
        var travelReportProcessor = new TravelReportProcessor(_fileStorage.Object, _excelService.Object, _travelQuery.Object, _idempotencyRepository.Object, _logger.Object);
        var result = travelReportProcessor.ProcessAsync(message, _cancellationToken);

        _logger.Verify(
                    x => x.Log(
                                LogLevel.Warning,
                                It.IsAny<EventId>(),
                                It.Is<It.IsAnyType>((v, t) =>
                                    v.ToString()!.Contains(messageWarning)),
                                It.IsAny<Exception>(),
                                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                    Times.Once);

        _logger.Verify(
                    x => x.Log(
                        LogLevel.Information,
                        It.IsAny<EventId>(),
                        It.IsAny<It.IsAnyType>(),
                        It.IsAny<Exception>(),
                        It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                    Times.Never);
    }

    [Fact]
    public void ProcessAsync_MessageAlreadyProcessed_ReturnWarnningAlreadyProcessed()
    {
        var messageWarning = "foi feito";
        var obj = new TravelReportEvent()
        {
            EventId = Guid.NewGuid(),
            OccurredAt = DateTime.UtcNow,
            DestinationId = Guid.NewGuid(),
            VehicleId = Guid.NewGuid()
        };
        var objJson = JsonSerializer.Serialize<TravelReportEvent>(obj);
        _idempotencyRepository.Setup(repo => repo.ThereIsAsync(It.IsAny<Guid>())).ReturnsAsync(true);

        var travelReportProcessor = new TravelReportProcessor(_fileStorage.Object, _excelService.Object, _travelQuery.Object, _idempotencyRepository.Object, _logger.Object);
        var result = travelReportProcessor.ProcessAsync(objJson, _cancellationToken);

        _logger.Verify(
                    x => x.Log(
                                LogLevel.Warning,
                                It.IsAny<EventId>(),
                                It.Is<It.IsAnyType>((v, t) =>
                                    v.ToString()!.Contains(messageWarning)),
                                It.IsAny<Exception>(),
                                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                    Times.Once);

        _idempotencyRepository.Verify(repo => repo.SaveAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public void ProcessAsync_ValidMessage_ReturnProcess()
    {
        var obj = new TravelReportEvent()
        {
            EventId = Guid.NewGuid(),
            OccurredAt = DateTime.UtcNow,
            DestinationId = Guid.NewGuid(),
            VehicleId = Guid.NewGuid()
        };
        var objJson = JsonSerializer.Serialize<TravelReportEvent>(obj);
        _idempotencyRepository.Setup(repo => repo.ThereIsAsync(It.IsAny<Guid>())).ReturnsAsync(false);

        var travelReportProcessor = new TravelReportProcessor(_fileStorage.Object, _excelService.Object, _travelQuery.Object, _idempotencyRepository.Object, _logger.Object);
        var result = travelReportProcessor.ProcessAsync(objJson, _cancellationToken);

        _idempotencyRepository.Verify(repo => repo.SaveAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        _idempotencyRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Guid>(), It.IsAny<DateTime>()), Times.Once);
    }

}
