using CarutiFlow.Domain.Handler;
using CarutiFlow.Domain.Handler.Entities;

namespace CarutiFlow.App;

public class Handler : BackgroundService
{
    private readonly ILogger<Handler> _logger;
    private readonly IWorkflowRepository _workflowRepository;
    private readonly IWorkflowStateRepository _workflowStateRepository;
    private readonly IQueue<NotificationHandlerRequest> _handlerQueue;

    private readonly HttpClient _client;

    public Handler(
        ILogger<Handler> logger,
        IWorkflowRepository workflowRepository,
        IWorkflowStateRepository workflowStateRepository,
        IQueue<NotificationHandlerRequest> handlerQueue)
    {
        _logger = logger;
        _workflowRepository = workflowRepository;
        _workflowStateRepository = workflowStateRepository;
        _handlerQueue = handlerQueue;
        _client = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5000")
        };
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var handlerRequest = await _handlerQueue.Consume();
            var workflowState = GetWorkflowState(handlerRequest);
            if (!workflowState.MoveNextRequest()) continue;

            var request = workflowState.Current;
            if (request!.Action.SatisfyExecuteCondition(workflowState.Previous))
            {
                //Make http request, put in a new queue
                request.ChangeState(EState.Processed);

                _logger.LogInformation(
                    "[Handler] Handled request {WorkflowName} Action {Action}", workflowState.Workflow.Name,
                    request.Action.Name);
            }

            var nextRequest = workflowState.GetNextRequest();
            if (nextRequest is null) continue;

            await _client.PostAsJsonAsync("/notifications", new NotificationRequest
            {
                WorkflowId = workflowState.Workflow.Id,
                WorkflowStateId = workflowState.Id,
                ScheduleSeconds = nextRequest.Action.ScheduleSeconds
            }, cancellationToken: stoppingToken);

            _logger.LogInformation(
                "[Handler] Schedule new notification {WorkflowName} Action {Action}", workflowState.Workflow.Name,
                nextRequest.Action.Name);
        }
    }

    private WorkflowState GetWorkflowState(NotificationHandlerRequest handlerRequest)
    {
        if (handlerRequest.WorkflowStateId is not null)
            return _workflowStateRepository.FindById(handlerRequest.WorkflowStateId.Value)!;

        var workflow = _workflowRepository.FindById(handlerRequest.WorkflowId);
        var workflowState = new WorkflowState(workflow!);
        _workflowStateRepository.Add(workflowState);
        return workflowState;
    }
}