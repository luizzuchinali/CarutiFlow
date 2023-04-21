namespace CarutiFlow.Domain.Handler.Entities;

public class Action : Entity
{
    public string Name { get; private set; }
    public EActionType Type { get; private set; }
    public Guid? PreviousActionId { get; private set; }

    public uint? ScheduleSeconds { get; set; }

    //Template, Content

    public EStateCheckOperator? Operator { get; private set; }
    public EState? PreviousStateCondition { get; private set; }


    public Action(string name, EActionType type, Guid? previousActionId = null)
    {
        Name = name;
        Type = type;
        PreviousActionId = previousActionId;
    }

    public Action(string name, EActionType type, Guid previousActionId,
        EState previousStateCondition, EStateCheckOperator @operator, uint scheduleSeconds) :
        this(name, type, previousActionId)
    {
        PreviousStateCondition = previousStateCondition;
        Operator = @operator;
        ScheduleSeconds = scheduleSeconds;
    }

    public bool SatisfyExecuteCondition(IVerifiableState? verifiable)
    {
        if (PreviousStateCondition is null)
            return true;

        if (verifiable == null)
            throw new InvalidOperationException("Verifiable can't be null if PreviousStateCondition is not null");

        return Operator switch
        {
            EStateCheckOperator.Has => verifiable.HasState(PreviousStateCondition.Value),
            EStateCheckOperator.NotHas => !verifiable.HasState(PreviousStateCondition.Value),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}

public enum EActionType
{
    SendPush,
    SendSms,
    SendEmail,
    SendWhatsApp,
    SendTelegram
}