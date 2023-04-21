using CarutiFlow.Domain.Scheduler;

namespace CarutiFlow.App;

public class AlarmClock : BackgroundService
{
    private readonly ILogger<AlarmClock> _logger;
    private readonly IQueue<NotificationRequest> _notificationQueue;
    private readonly IScheduledRequestRepository _scheduledRequestRepository;

    public AlarmClock(
        ILogger<AlarmClock> logger,
        IQueue<NotificationRequest> notificationQueue,
        IScheduledRequestRepository scheduledRequestRepository)
    {
        _logger = logger;
        _notificationQueue = notificationQueue;
        _scheduledRequestRepository = scheduledRequestRepository;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var requests = _scheduledRequestRepository.GetAllReadyToSend();
            foreach (var req in requests)
            {
                _notificationQueue.Enqueue(new NotificationRequest
                {
                    WorkflowId = req.WorkflowId,
                    WorkflowStateId = req.WorkflowStateId
                });

                _logger.LogInformation(
                    "[AlarmClock] Enqueueded new notification request to workflow {WorkflowId}", req.WorkflowId);
                
                _scheduledRequestRepository.Remove(req.Id);
            }

            await Task.Delay(10000, stoppingToken);
        }
    }
}