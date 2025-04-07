# NanoMediator Library Documentation

The `NanoMediator` is a lightweight CQRS (Command Query Responsibility Segregation) pattern implementation for .NET, providing a minimal mediator without third-party dependencies.

---

## 🔧 Setup

### 1. Register the Mediator

In your `Startup.cs` or `Program.cs` for minimal hosting (ASP.NET Core or Console App), register the NanoMediator with:

```csharp
services.AddNanoMediator(typeof(Program).Assembly);
```

You can pass any type from the assembly that contains your handlers. The method will scan and register all handlers implementing `IDataRequestHandler<,>`.

---

## 🧩 Core Interfaces

### `IDataRequest<TDataResponse>`

Represents a request that returns a response of type `TDataResponse`.

```csharp
public interface IDataRequest<TDataResponse> { }
```

### `IDataRequestHandler<TDataRequest, TDataResponse>`

Handler interface for processing `IDataRequest`.

```csharp
public interface IDataRequestHandler<TDataRequest, TDataResponse>
    where TDataRequest : IDataRequest<TDataResponse>
{
    Task<TDataResponse> HandleAsync(TDataRequest request, CancellationToken cancellationToken = default);
}
```

---

## ⚙️ Using the Mediator

### Inject `NanoMediator`

Use constructor injection to access `NanoMediator`:

```csharp
public class SomeService
{
    private readonly INanoMediator _mediator;

    public SomeService(INanoMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task DoWorkAsync()
    {
        var result = await _mediator.SendAsync(new SomeQuery());
    }
}
```

---

## 📦 Example

### Define a Query

```csharp
public class CurrentTimeQuery : IDataRequest<string> { }
```

### Implement the Handler

```csharp
public class CurrentTimeQueryHandler : IDataRequestHandler<CurrentTimeQuery, string>
{
    public Task<string> HandleAsync(CurrentTimeQuery request, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(DateTime.Now.ToString("T"));
    }
}
```

### Send the Query

```csharp
var currentTime = await _mediator.SendAsync(new CurrentTimeQuery());
Console.WriteLine(currentTime);
```


---

## 🧪 Sample Projects

This repository includes:
- `NanoMediatorConsoleSample` — demonstrates basic query/command usage.
- `NanoMediatorAspNetSample` — shows integration with EF Core and Web APIs.

---

## ✅ Benefits

- Minimal abstraction
- No third-party packages required
- Ideal for microservices and lightweight applications

---

## 📁 Source Files

- `NanoMediator.cs`: Nano Mediator implementation
- `INanoMediator.cs` : Nano Mediator interface
- `IDataRequest.cs`: Request interface
- `IDataRequestHandler.cs`: Handler interface
- `ServiceCollectionExtensions.cs`: Dependency injection extensions

---

## 📝 License

MIT or similar (check repository for actual license)