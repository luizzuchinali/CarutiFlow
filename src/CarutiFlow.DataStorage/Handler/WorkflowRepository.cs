namespace CarutiFlow.DataStorage.Handler;

public class WorkflowRepository : IWorkflowRepository
{
    private IList<Workflow> _workflows = new List<Workflow>();

    public void Add(Workflow workflow)
    {
        ArgumentNullException.ThrowIfNull(workflow);
        _workflows.Add(workflow);
    }

    public Workflow? FindById(Guid id) =>
        _workflows.FirstOrDefault(x => x.Id.Equals(id));
}