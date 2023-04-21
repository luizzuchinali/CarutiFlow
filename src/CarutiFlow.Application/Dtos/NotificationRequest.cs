namespace CarutiFlow.App.Dtos;

public class NotificationRequest
{
    public Guid WorkflowId { get; set; }
    public Guid? WorkflowStateId { get; set; }
    public uint? ScheduleSeconds { get; set; }
}