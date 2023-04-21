namespace CarutiFlow.Domain.Handler.Entities;

public class Request : Entity, IVerifiableState
{
    public EState CurrentState { get; private set; }
    public IList<EState> PreviousStates { get; private set; }
    public Action Action { get; private set; }

    public Request(EState state, Action action)
    {
        CurrentState = state;
        Action = action;
        PreviousStates = new List<EState>();
    }

    public void ChangeState(EState newState)
    {
        PreviousStates.Add(CurrentState);
        CurrentState = newState;
    }

    //Vars, Data
    public bool HasState(EState state) =>
        CurrentState == state || PreviousStates.Contains(state);
}