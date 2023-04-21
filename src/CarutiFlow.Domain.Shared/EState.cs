namespace CarutiFlow.Domain.Shared;

public enum EState
{
    Requested,
    Scheduled,
    Discarded,
    Processed,
    Sent,
    Received,
    NotDelivered,


    //E-mail status
    Opened,
    Clicked
}