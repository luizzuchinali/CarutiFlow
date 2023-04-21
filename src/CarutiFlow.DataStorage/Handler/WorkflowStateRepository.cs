namespace CarutiFlow.DataStorage.Handler;

public class WorkflowStateRepository : IWorkflowStateRepository
{
    private IList<WorkflowState> _workflows = new List<WorkflowState>();


    public void Add(WorkflowState workflowState)
    {
        ArgumentNullException.ThrowIfNull(workflowState);
        _workflows.Add(workflowState);
    }

    public WorkflowState? FindById(Guid id) =>
        _workflows.FirstOrDefault(x => x.Id.Equals(id));
}