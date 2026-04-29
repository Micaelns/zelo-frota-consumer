namespace Application.Contracts.Excel;

public class ExcelDocument
{
    public string Name { get; set; }
    public List<ExcelSheet> Sheets { get; set; } = new();
}
