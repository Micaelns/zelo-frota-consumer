using Application.Contracts;
using Application.Contracts.Excel;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Infra.External.Excel;

public class ExcelService : IExcelService
{
    public byte[] Generate()
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Relatório");

        worksheet.Cell(1, 1).Value = "VehicleId";
        worksheet.Cell(1, 2).Value = "TravelId";
        worksheet.Cell(1, 3).Value = "Data";

        worksheet.Cell(2, 1).Value = "Coluna1";
        worksheet.Cell(2, 2).Value = "Coluna2";
        worksheet.Cell(2, 3).Value = DateTime.Now;

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);

        return stream.ToArray();
    }

    public byte[] Generate(ExcelDocument document)
    {
        using var workbook = new XLWorkbook();
        foreach (var sheet in document.Sheets)
        {
            var ws = workbook.Worksheets.Add(sheet.Name);
            PrepareHeader(ws, sheet);
            PrepareBody(ws, sheet);
        }
        using var stream = new MemoryStream();
        workbook.SaveAs(stream);

        return stream.ToArray();
    }

    private static void PrepareHeader(IXLWorksheet? ws, ExcelSheet sheet)
    {
        if (ws == null) return;

        for (int col = 0; col < sheet.Columns.Count; col++)
        {
            ws.Cell(1, col + 1).Value = sheet.Columns[col];
        }
    }

    private static void PrepareBody(IXLWorksheet? ws, ExcelSheet sheet)
    {
        if (ws == null) return;

        for (int row = 0; row < sheet.Rows.Count; row++)
        {
            for (int col = 0; col < sheet.Rows[row].Values.Count; col++)
            {
                ws.Cell(row + 2, col + 1).Value = sheet.Rows[row].Values[col]??string.Empty;
            }
        }
    }

}
