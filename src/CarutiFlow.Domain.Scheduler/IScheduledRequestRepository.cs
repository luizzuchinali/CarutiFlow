namespace CarutiFlow.Domain.Scheduler;

public interface IScheduledRequestRepository
{
    void Add(ScheduledRequest request);
    IEnumerable<ScheduledRequest> GetAllReadyToSend();

    void Remove(Guid id);
}