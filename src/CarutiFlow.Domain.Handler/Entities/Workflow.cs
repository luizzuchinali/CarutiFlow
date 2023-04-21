namespace CarutiFlow.Domain.Handler.Entities;

public class Workflow : Entity
{
    public string Name { get; private set; }
    public IList<Action> Actions { get; private set; }

    public Workflow(string name)
    {
        Name = name;
        Actions = new List<Action>();
    }

    public void AddAction(Action action)
    {
        ArgumentNullException.ThrowIfNull(action);

        Actions.Add(action);
    }

    public void AddActions(params Action[] actions)
    {
        ArgumentNullException.ThrowIfNull(actions);

        foreach (var action in actions)
        {
            AddAction(action);
        }
    }
}