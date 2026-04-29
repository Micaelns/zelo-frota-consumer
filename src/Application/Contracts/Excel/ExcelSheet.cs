namespace Application.Contracts.Excel;

public class ExcelSheet
{
    public string Name { get; set; } = string.Empty;
    public List<string> Columns { get; set; } = [];
    public List<ExcelRow> Rows { get; set; } = [];
}
