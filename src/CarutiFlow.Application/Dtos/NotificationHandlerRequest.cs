namespace CarutiFlow.App.Dtos;

public class NotificationHandlerRequest
{
    public Guid WorkflowId { get; set; }
    public Guid? WorkflowStateId { get; set; }
}