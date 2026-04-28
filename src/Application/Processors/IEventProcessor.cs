namespace Application.Processors;

public interface IEventProcessor
{
    public Task ProcessAsync(string message, CancellationToken cancellationToken);
}
