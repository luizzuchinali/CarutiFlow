namespace CarutiFlow.DataStorage;

public class AppQueue<T> : IQueue<T> where T : class
{
    private readonly Queue<T> _queue = new();

    public async Task<T> Consume()
    {
        await Task.Run(() =>
        {
            while (_queue.Count == 0)
            {
            }
        });

        return _queue.Dequeue();
    }


    public void Enqueue(T data) =>
        _queue.Enqueue(data);
}