namespace CarutiFlow.Domain.Handler;

public interface IWorkflowRepository
{
    void Add(Workflow workflow);
    Workflow? FindById(Guid id);
}