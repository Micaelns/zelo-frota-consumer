using Domain.Entities;

namespace Application.Contracts;

public interface IExcelService
{
    public byte[] Generate(ExcelDocument document);
}
