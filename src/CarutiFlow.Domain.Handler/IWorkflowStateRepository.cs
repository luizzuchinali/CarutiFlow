namespace CarutiFlow.Domain.Handler;

public interface IWorkflowStateRepository
{
    void Add(WorkflowState workflowState);
    WorkflowState? FindById(Guid id);
}