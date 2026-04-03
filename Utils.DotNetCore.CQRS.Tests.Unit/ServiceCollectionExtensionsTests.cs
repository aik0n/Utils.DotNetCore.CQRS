using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Utils.DotNetCore.CQRS;
using Xunit;

namespace Utils.DotNetCore.CQRS.Tests.Unit;

[Trait("Category", "Unit")]
public class ServiceCollectionExtensionsTests
{
    private static readonly Assembly TestAssembly =
        typeof(ServiceCollectionExtensionsTests).Assembly;

    private static readonly HashSet<Type> KnownConcreteHandlerTypes = new()
    {
        typeof(SampleRequestHandler),
        typeof(CancellationCapturingHandler),
        typeof(RequestCapturingHandler),
        typeof(MultiHandler),
    };

    [Fact]
    public void AddNanoMediator_Always_ReturnsSameServiceCollectionInstance()
    {
        var services = new ServiceCollection();

        var returned = services.AddNanoMediator(TestAssembly);

        returned.Should().BeSameAs(services);
    }

    [Fact]
    public void AddNanoMediator_Always_RegistersINanoMediatorAsScoped()
    {
        var services = new ServiceCollection();

        services.AddNanoMediator(TestAssembly);

        services.Should().ContainSingle(d =>
            d.ServiceType == typeof(INanoMediator) &&
            d.ImplementationType == typeof(NanoMediator) &&
            d.Lifetime == ServiceLifetime.Scoped);
    }

    [Fact]
    public void AddNanoMediator_AssemblyWithHandlers_RegistersAllConcreteHandlers()
    {
        var services = new ServiceCollection();

        services.AddNanoMediator(TestAssembly);

        foreach (var handlerType in KnownConcreteHandlerTypes)
        {
            services.Should().Contain(d => d.ImplementationType == handlerType,
                because: $"{handlerType.Name} is a concrete handler and must be discovered");
        }
    }

    [Fact]
    public void AddNanoMediator_AssemblyWithHandlers_RegistersHandlersAsScoped()
    {
        var services = new ServiceCollection();

        services.AddNanoMediator(TestAssembly);

        var handlerDescriptors = services
            .Where(d => d.ServiceType.IsGenericType &&
                        d.ServiceType.GetGenericTypeDefinition() == typeof(IDataRequestHandler<,>))
            .ToList();

        handlerDescriptors.Should().NotBeEmpty();
        handlerDescriptors.Should().AllSatisfy(d =>
            d.Lifetime.Should().Be(ServiceLifetime.Scoped));
    }

    [Fact]
    public void AddNanoMediator_ConcreteHandler_RegisteredUnderCorrectServiceType()
    {
        var services = new ServiceCollection();

        services.AddNanoMediator(TestAssembly);

        services.Should().Contain(d =>
            d.ServiceType == typeof(IDataRequestHandler<SampleRequest, string>) &&
            d.ImplementationType == typeof(SampleRequestHandler) &&
            d.Lifetime == ServiceLifetime.Scoped);
    }

    [Fact]
    public void AddNanoMediator_AbstractHandler_IsNotRegistered()
    {
        var services = new ServiceCollection();

        services.AddNanoMediator(TestAssembly);

        services.Should().NotContain(d => d.ImplementationType == typeof(AbstractHandler));
    }

    [Fact]
    public void AddNanoMediator_HandlerInterface_IsNotRegistered()
    {
        var services = new ServiceCollection();

        services.AddNanoMediator(TestAssembly);

        services.Should().NotContain(d => d.ImplementationType == typeof(IExtraHandler));
    }

    [Fact]
    public void AddNanoMediator_HandlerImplementingMultipleInterfaces_RegisteredOncePerInterface()
    {
        var services = new ServiceCollection();

        services.AddNanoMediator(TestAssembly);

        var multiHandlerDescriptors = services
            .Where(d => d.ImplementationType == typeof(MultiHandler))
            .ToList();

        multiHandlerDescriptors.Should().HaveCount(2);

        multiHandlerDescriptors.Should().Contain(d =>
            d.ServiceType == typeof(IDataRequestHandler<MultiRequest, bool>));

        multiHandlerDescriptors.Should().Contain(d =>
            d.ServiceType == typeof(IDataRequestHandler<MultiRequestAlt, double>));
    }

    [Fact]
    public void AddNanoMediator_AssemblyWithNoHandlers_StillRegistersINanoMediatorOnly()
    {
        var emptyAssembly = typeof(INanoMediator).Assembly;
        var services = new ServiceCollection();

        services.AddNanoMediator(emptyAssembly);

        services.Should().ContainSingle(d =>
            d.ServiceType == typeof(INanoMediator) &&
            d.Lifetime == ServiceLifetime.Scoped);

        services.Should().NotContain(d =>
            d.ServiceType.IsGenericType &&
            d.ServiceType.GetGenericTypeDefinition() == typeof(IDataRequestHandler<,>));
    }
}
