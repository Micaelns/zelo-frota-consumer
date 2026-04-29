using Application.Contracts;

namespace Infra.External.Excel;

public class LocalFileStorage: IFileStorage
{
    public Task SaveAsync(string fileName, byte[] content)
    {
        File.WriteAllBytes(Path.Combine("Files", fileName), content);
        return Task.CompletedTask;
    }
}
