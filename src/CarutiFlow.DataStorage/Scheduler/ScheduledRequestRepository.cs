namespace CarutiFlow.DataStorage.Scheduler;

public class ScheduledRequestRepository : IScheduledRequestRepository
{
    private IList<ScheduledRequest> _requests = new List<ScheduledRequest>();

    public void Add(ScheduledRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        _requests.Add(request);
    }

    public IEnumerable<ScheduledRequest> GetAllReadyToSend()
    {
        var now = DateTime.UtcNow;
        return _requests.Where(x => x.ScheduledDate <= now).ToList();
    }

    public void Remove(Guid id)
    {
        _requests.Remove(_requests.First(x => x.Id.Equals(id)));
    }
}