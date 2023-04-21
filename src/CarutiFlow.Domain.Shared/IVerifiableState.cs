using CarutiFlow.Domain.Shared;

namespace CarutiFlow.Domain.Shared;

public interface IVerifiableState
{
    bool HasState(EState state);
}