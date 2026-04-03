[![License: MIT](https://img.shields.io/badge/license-MIT-FFD700.svg?style=flat)](https://opensource.org/licenses/MIT)
![Tag](https://img.shields.io/github/v/tag/aik0n/Utils.DotNetCore.CQRS?label=version&style=flat&color=090&sort=semver)
![Repo Size](https://img.shields.io/github/repo-size/aik0n/Utils.DotNetCore.CQRS?style=flat&color=036)
![Stars](https://img.shields.io/github/stars/aik0n/Utils.DotNetCore.CQRS?style=flat&color=DAA520)

# NanoMediator Library Documentation

The `NanoMediator` is a lightweight CQRS (Command Query Responsibility Segregation) pattern implementation for .NET, providing a minimal mediator without third-party dependencies.

---

## 🔧 Setup

### 1. Register the Mediator  

It is possible to use a NuGet package, as example:  
```
dotnet add package Utils.DotNetCore.CQRS --version 1.0.3
```

> **Breaking change in v1.0.3:** The root namespace was renamed from `utils_netcore_cqrs` to `Utils.DotNetCore.CQRS`. If you are upgrading from v1.0.2 or earlier, update all `using utils_netcore_cqrs;` directives to `using Utils.DotNetCore.CQRS;`.

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
    Task<TDataResponse> Handle(TDataRequest request, CancellationToken cancellationToken = default);
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
        var result = await _mediator.Send(new SomeQuery());
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
    public Task<string> Handle(CurrentTimeQuery request, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(DateTime.Now.ToString("T"));
    }
}
```

### Send the Query

```csharp
var currentTime = await _mediator.Send(new CurrentTimeQuery());
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

This project is licensed under the [MIT License](./LICENSE)
