using Application.Contracts.Excel;

namespace Application.Contracts;

public interface IExcelService
{
    public byte[] Generate();
    public byte[] Generate(ExcelDocument document);
}
