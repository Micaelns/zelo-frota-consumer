using Application.Contracts;
using Application.Contracts.Excel;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace Application.Processors;

public class TravelReportProcessor(IFileStorage fileStorage, IExcelService excelService, ILogger<TravelReportProcessor> logger) : IEventProcessor
{
    private readonly ILogger<TravelReportProcessor> _logger = logger;
    private readonly IFileStorage _fileStorage = fileStorage;
    private readonly IExcelService _excelService = excelService;

    public async Task ProcessAsync(
        string message,
        CancellationToken cancellationToken)
    {
        if (message == null)
        {
            _logger.LogWarning("Mensagem nula ou invalida para processamento");
        }
        _logger.LogInformation("Processando mensagem: {@message}", message);

        var data = PopulateDocument();
        var file = _excelService.Generate(data);
        await _fileStorage.SaveAsync(data.Name, file);

        await Task.CompletedTask;
    }

    public ExcelDocument PopulateDocument()
    {
        return new ExcelDocument()
        {
            Name = "JAN_2026.xlsx",
            Sheets = {
                new ExcelSheet { 
                        Name = "Carro QKS 1C57",
                        Columns = {"Data","Km_inicial","Km_final","Consumo_medio" },
                        Rows =
                        {
                            new ExcelRow { Values = {"12/01/2026",10000,12000,2.5 } },
                            new ExcelRow { Values = {"12/01/2026",12000,16000,2.2 } },
                            new ExcelRow { Values = {"13/01/2026",16000,19000,2.7 } },
                            new ExcelRow { Values = {"15/01/2026",19000,21000,2.5 } }
                        }
                },
                new ExcelSheet {
                        Name = "Carro KKK 1A00",
                        Columns = {"Data","Km_inicial","Km_final","Consumo_medio" },
                        Rows = 
                        {
                            new ExcelRow { Values = {"13/01/2026",11000,12000,2.5 } },
                            new ExcelRow(),
                            new ExcelRow { Values = {"14/01/2026", 12000, 19000 } },
                            new ExcelRow { Values = {"14/01/2026" } }
                        }
                },
                new ExcelSheet {
                        Name = "Carro SSS 1B11",
                        Columns = {"Data","Km_inicial","Km_final","Consumo_medio" },
                        Rows = 
                        {
                            new ExcelRow { Values = {"16/01/2026",1000,1200,2.5 } },
                            new ExcelRow { Values = {"16/01/2026",1200,2100,2 } },
                            new ExcelRow { Values = {"17/01/2026",2100,3000,3 } },
                            new ExcelRow { Values = {"19/01/2026",3000,5000,2.8 } }
                        }
                },
            }
        };
    }
}
