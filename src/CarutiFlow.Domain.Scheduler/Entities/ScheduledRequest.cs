namespace CarutiFlow.Domain.Scheduler.Entities;

public class ScheduledRequest : Entity
{
    public Guid WorkflowId { get; set; }
    public Guid? WorkflowStateId { get; set; }
    public DateTime ScheduledDate { get; set; }

    public ScheduledRequest(Guid workflowId, DateTime scheduledDate, Guid? workflowStateId = null)
    {
        WorkflowId = workflowId;
        ScheduledDate = scheduledDate;
        WorkflowStateId = workflowStateId;
    }
}