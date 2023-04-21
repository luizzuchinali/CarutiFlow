namespace CarutiFlow.DataStorage.Handler;

public class WorkflowRepositoryFactory
{
    public static WorkflowRepository Create()
    {
        var repository = new WorkflowRepository();

        var emailAction = new Action("Send E-mail", EActionType.SendEmail);
        var smsAction = new Action("Send SMS",
            EActionType.SendSms,
            emailAction.Id,
            EState.Opened,
            EStateCheckOperator.NotHas, 15);

        var workflow = new Workflow("Workflow A")
        {
            Id = Guid.Parse("2320B255-DFED-4A02-A7BD-F8C4427B3F9A")
        };
        workflow.AddActions(emailAction, smsAction);

        repository.Add(workflow);
        return repository;
    }
}