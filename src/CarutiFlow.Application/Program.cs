using CarutiFlow.DataStorage;
using CarutiFlow.DataStorage.Handler;
using CarutiFlow.DataStorage.Scheduler;
using CarutiFlow.Domain.Handler;
using CarutiFlow.Domain.Scheduler;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(typeof(IQueue<>), typeof(AppQueue<>));

//Scheduler
builder.Services.AddSingleton<IScheduledRequestRepository, ScheduledRequestRepository>();

builder.Services.AddHostedService<Scheduler>();

//Handler
builder.Services.AddSingleton<IWorkflowRepository>(WorkflowRepositoryFactory.Create());
builder.Services.AddSingleton<IWorkflowStateRepository, WorkflowStateRepository>();

builder.Services.AddHostedService<Handler>();

//AlarmClock
builder.Services.AddHostedService<AlarmClock>();

var app = builder.Build();

app.MapPost("/notifications",
    (NotificationRequest request, IQueue<NotificationRequest> queue) =>
    {
        queue.Enqueue(request);
        return Results.Ok();
    });

app.Run();