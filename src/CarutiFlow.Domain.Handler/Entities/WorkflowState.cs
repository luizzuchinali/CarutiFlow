namespace CarutiFlow.Domain.Handler.Entities;

public class WorkflowState : Entity
{
    public Workflow Workflow { get; private set; }
    public IList<Request> Requests { get; private set; }

    public Request? Previous { get; private set; }
    public Request? Current { get; private set; }

    public WorkflowState(Workflow workflow)
    {
        Workflow = workflow;
        Requests = new List<Request>();
    }

    public Request? GetNextRequest()
    {
        var lastReq = Requests.LastOrDefault();
        if (lastReq is null)
            return null;

        var nextAction = Workflow.Actions.FirstOrDefault(x => x.PreviousActionId == lastReq.Action.Id);
        if (nextAction is null)
            return null;

        return new Request(EState.Scheduled, nextAction);
    }

    public bool MoveNextRequest()
    {
        var lastReq = Requests.LastOrDefault();
        if (lastReq is null)
        {
            var firstAction = Workflow.Actions.First();
            var requested = new Request(EState.Requested, firstAction);
            Requests.Add(requested);
            Current = requested;
            return true;
        }

        var nextAction = Workflow.Actions.FirstOrDefault(x => x.PreviousActionId == lastReq.Action.Id);
        if (nextAction is null)
            return false;

        var scheduled = new Request(EState.Scheduled, nextAction);
        Requests.Add(scheduled);
        Previous = Current;
        Current = scheduled;
        return true;
    }
}