using Application.DTOs;
using Domain.Entities;

namespace Application.Mappers;

public class TravelMapper
{
    public static ExcelDocument ToExcelDocument(List<TravelDTO> data)
    {
        var document = new ExcelDocument
        {
            Name = $"viagens_{DateTime.UtcNow:yyyy_MM_dd-HHmm}.xlsx"
        };

        var grouped = data.GroupBy(t => t.VehiclePlate);

        foreach (var group in grouped)
        {
            var sheet = new ExcelSheet
            {
                Name = NormalizeSheetName(group.Key),
                Columns =
                {
                    "Destino",
                    "Km Inicial",
                    "Km Final",
                    "Autonomia",
                    "Início",
                    "Fim"
                }
            };

            foreach (var item in group)
            {
                sheet.Rows.Add(new ExcelRow
                {
                    Values =
                    {
                        item.TravelDestination,
                        item.StartedMileage,
                        item.FinishedMileage,
                        item.Autonomy,
                        item.Start,
                        item.End
                    }
                });
            }

            document.Sheets.Add(sheet);
        }

        if(document.Sheets.Count == 0)
        {
            var sheet = new ExcelSheet
            {
                Name = "XXX0X00",
                Columns =
                {
                    "Destino",
                    "Km Inicial",
                    "Km Final",
                    "Autonomia",
                    "Início",
                    "Fim"
                }
            };
            document.Sheets.Add(sheet);
        }

        return document;
    }

    private static string NormalizeSheetName(string name)
    {
        var invalidChars = new[] { '\\', '/', '?', '*', '[', ']' };

        foreach (var c in invalidChars)
        {
            name = name.Replace(c, '_');
        }

        return name.Length > 31
            ? name.Substring(0, 31)
            : name;
    }
}
