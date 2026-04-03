using Utils.DotNetCore.CQRS;

namespace Utils.DotNetCore.CQRS.Tests.Unit;

public sealed class SampleRequest : IDataRequest<string> { }
public sealed class AnotherRequest : IDataRequest<int> { }
public sealed class MultiRequest : IDataRequest<bool> { }
public sealed class MultiRequestAlt : IDataRequest<double> { }

public sealed class SampleRequestHandler : IDataRequestHandler<SampleRequest, string>
{
    public Task<string> Handle(SampleRequest request, CancellationToken cancellationToken)
        => Task.FromResult("handled");
}

public sealed class CancellationCapturingHandler : IDataRequestHandler<AnotherRequest, int>
{
    public CancellationToken CapturedToken { get; private set; }

    public Task<int> Handle(AnotherRequest request, CancellationToken cancellationToken)
    {
        CapturedToken = cancellationToken;
        return Task.FromResult(42);
    }
}

public sealed class RequestCapturingHandler : IDataRequestHandler<SampleRequest, string>
{
    public IDataRequest<string>? CapturedRequest { get; private set; }

    public Task<string> Handle(SampleRequest request, CancellationToken cancellationToken)
    {
        CapturedRequest = request;
        return Task.FromResult("captured");
    }
}

public sealed class MultiHandler
    : IDataRequestHandler<MultiRequest, bool>,
      IDataRequestHandler<MultiRequestAlt, double>
{
    public Task<bool> Handle(MultiRequest request, CancellationToken cancellationToken)
        => Task.FromResult(true);

    public Task<double> Handle(MultiRequestAlt request, CancellationToken cancellationToken)
        => Task.FromResult(3.14);
}

// Must be in test assembly so scanner encounters them — used in negative-assertion tests
public abstract class AbstractHandler : IDataRequestHandler<SampleRequest, string>
{
    public abstract Task<string> Handle(SampleRequest request, CancellationToken cancellationToken);
}

public interface IExtraHandler : IDataRequestHandler<SampleRequest, string> { }
