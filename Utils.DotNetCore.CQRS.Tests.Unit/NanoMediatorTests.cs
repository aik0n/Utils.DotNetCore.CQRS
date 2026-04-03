using FluentAssertions;
using NSubstitute;
using Utils.DotNetCore.CQRS;
using Xunit;

namespace Utils.DotNetCore.CQRS.Tests.Unit;

[Trait("Category", "Unit")]
public class NanoMediatorTests
{
    [Fact]
    public void Constructor_NullServiceProvider_ThrowsArgumentNullException()
    {
        var act = () => new NanoMediator(null!);

        act.Should().ThrowExactly<ArgumentNullException>()
           .WithParameterName("serviceProvider");
    }

    [Fact]
    public async Task Send_RegisteredHandler_ReturnsHandlerResult()
    {
        var handler = new SampleRequestHandler();
        var serviceProvider = Substitute.For<IServiceProvider>();
        serviceProvider
            .GetService(typeof(IDataRequestHandler<SampleRequest, string>))
            .Returns(handler);

        var mediator = new NanoMediator(serviceProvider);

        var result = await mediator.Send(new SampleRequest());

        result.Should().Be("handled");
    }

    [Fact]
    public async Task Send_WithCancellationToken_ForwardsTokenToHandler()
    {
        using var cts = new CancellationTokenSource();
        var token = cts.Token;

        var capturingHandler = new CancellationCapturingHandler();
        var serviceProvider = Substitute.For<IServiceProvider>();
        serviceProvider
            .GetService(typeof(IDataRequestHandler<AnotherRequest, int>))
            .Returns(capturingHandler);

        var mediator = new NanoMediator(serviceProvider);

        await mediator.Send(new AnotherRequest(), token);

        capturingHandler.CapturedToken.Should().Be(token);
    }

    [Fact]
    public async Task Send_WithRequest_PassesExactRequestInstanceToHandler()
    {
        var expectedRequest = new CapturingRequest();

        var capturingHandler = new RequestCapturingHandler();
        var serviceProvider = Substitute.For<IServiceProvider>();
        serviceProvider
            .GetService(typeof(IDataRequestHandler<CapturingRequest, string>))
            .Returns(capturingHandler);

        var mediator = new NanoMediator(serviceProvider);

        await mediator.Send(expectedRequest);

        capturingHandler.CapturedRequest.Should().BeSameAs(expectedRequest);
    }

    [Fact]
    public async Task Send_NoHandlerRegistered_ThrowsInvalidOperationExceptionWithTypeName()
    {
        var serviceProvider = Substitute.For<IServiceProvider>();
        serviceProvider.GetService(Arg.Any<Type>()).Returns((object?)null);

        var mediator = new NanoMediator(serviceProvider);

        var act = () => mediator.Send(new SampleRequest());

        await act.Should().ThrowExactlyAsync<InvalidOperationException>()
            .WithMessage($"*{nameof(SampleRequest)}*");
    }

    [Fact]
    public async Task Send_NullRequest_ThrowsArgumentNullException()
    {
        var serviceProvider = Substitute.For<IServiceProvider>();
        var mediator = new NanoMediator(serviceProvider);

        var act = () => mediator.Send<string>(null!);

        await act.Should().ThrowExactlyAsync<ArgumentNullException>()
            .WithParameterName("request");
    }

    [Fact]
    public async Task Send_WithDefaultCancellationToken_PassesDefaultTokenToHandler()
    {
        var capturingHandler = new CancellationCapturingHandler();
        var serviceProvider = Substitute.For<IServiceProvider>();
        serviceProvider
            .GetService(typeof(IDataRequestHandler<AnotherRequest, int>))
            .Returns(capturingHandler);

        var mediator = new NanoMediator(serviceProvider);

        await mediator.Send(new AnotherRequest());

        capturingHandler.CapturedToken.Should().Be(CancellationToken.None);
    }
}
