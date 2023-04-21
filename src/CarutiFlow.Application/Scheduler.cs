using CarutiFlow.Domain.Scheduler;
using CarutiFlow.Domain.Scheduler.Entities;

namespace CarutiFlow.App;

public class Scheduler : BackgroundService
{
    private readonly ILogger<Scheduler> _logger;
    private readonly IQueue<NotificationRequest> _notificationQueue;
    private readonly IQueue<NotificationHandlerRequest> _handlerQueue;
    private readonly IScheduledRequestRepository _scheduledRequestRepository;

    public Scheduler(
        ILogger<Scheduler> logger,
        IQueue<NotificationRequest> notificationQueue,
        IQueue<NotificationHandlerRequest> handlerQueue,
        IScheduledRequestRepository scheduledRequestRepository)
    {
        _logger = logger;
        _notificationQueue = notificationQueue;
        _handlerQueue = handlerQueue;
        _scheduledRequestRepository = scheduledRequestRepository;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var notificationRequest = await _notificationQueue.Consume();

            if (notificationRequest.ScheduleSeconds is not null)
            {
                var scheduledDate = DateTime.UtcNow.AddSeconds(notificationRequest.ScheduleSeconds.Value);
                var scheduledRequest = new ScheduledRequest(
                    notificationRequest.WorkflowId,
                    scheduledDate,
                    notificationRequest.WorkflowStateId);
                _scheduledRequestRepository.Add(scheduledRequest);
                _logger.LogInformation("[Scheduler] Scheduled new notification request to workflow {WorkflowId}",
                    notificationRequest.WorkflowId.ToString());
                continue;
            }

            var handlerRequest = new NotificationHandlerRequest
            {
                WorkflowId = notificationRequest.WorkflowId,
                WorkflowStateId = notificationRequest.WorkflowStateId
            };
            _handlerQueue.Enqueue(handlerRequest);

            _logger.LogInformation("[Scheduler] Enqueueded new notification request to workflow {WorkflowId}",
                notificationRequest.WorkflowId.ToString());
        }
    }
}