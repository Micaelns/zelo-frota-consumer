namespace Application.Contracts;

public interface IFileStorage
{
    Task SaveAsync(string fileName, byte[] content);
}
