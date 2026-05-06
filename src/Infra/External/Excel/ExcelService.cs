using Application.Contracts;
using ClosedXML.Excel;
using Domain.Entities;

namespace Infra.External.Excel;

public class ExcelService : IExcelService
{
    public byte[] Generate(ExcelDocument document)
    {
        using var workbook = new XLWorkbook();
        foreach (var sheet in document.Sheets)
        {
            var ws = workbook.Worksheets.Add(sheet.Name);
            ws.Cell(1, 1).InsertData(new[] { sheet.Columns });
            PrepareBody(ws, sheet);
        }
        using var stream = new MemoryStream();
        workbook.SaveAs(stream);

        return stream.ToArray();
    }

    private static void PrepareBody(IXLWorksheet ws, ExcelSheet sheet)
    {

        var data = new List<object[]>();
        foreach (var row in sheet.Rows)
        {
            var values = new object[sheet.Columns.Count];
            for (int col = 0; col < sheet.Columns.Count; col++)
            {
                var value = col < row.Values.Count
                            ? row.Values[col]
                            : null;

                values[col] = value ?? string.Empty;
            }

            data.Add(values);
        }
        ws.Cell(2, 1).InsertData(data);
    }

}
