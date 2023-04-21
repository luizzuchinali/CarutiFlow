namespace CarutiFlow.Domain.Shared;

public interface IQueue<T> where T : class
{
    public Task<T> Consume();

    public void Enqueue(T data);
}