namespace StoreOnline.Domain.Ports;
public interface IUnitOfWork
{
    Task SaveAsync(CancellationToken? cancellationToken);
}
