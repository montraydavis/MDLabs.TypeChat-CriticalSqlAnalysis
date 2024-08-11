# Program.cs

## Overview
`Program` is the entry point class for the Critical SQL Analysis Tool application. It sets up the host builder, configures services, and dependencies. This class is part of the `MDLabs.CriticalSqlAnalysis` namespace.

## Class Definition
```csharp
public class Program
```

## Dependencies
- `Autofac`
- `Autofac.Extensions.DependencyInjection`
- `MDLabs.CriticalSqlAnalysis.Agents`
- `MDLabs.CriticalSqlAnalysis.Handlers`
- `MDLabs.CriticalSqlAnalysis.Loaders`
- `MDLabs.CriticalSqlAnalysis.Models.Options`
- `MediatR`
- `Microsoft.Extensions.Configuration`
- `Microsoft.Extensions.DependencyInjection`
- `Microsoft.Extensions.Hosting`
- `Microsoft.Extensions.Options`

## Methods

### Main
```csharp
public static void Main(string[] args)
```
The entry point of the application. It builds and runs the host, and includes basic error handling to log any unhandled exceptions.

### CreateHostBuilder
```csharp
public static IHostBuilder CreateHostBuilder(string[] args)
```
Creates and configures the host builder. It:
1. Uses the default builder.
2. Configures Autofac as the service provider factory.
3. Loads configuration from "appsettings.json".
4. Configures services, including MediatR and the `Worker` as a hosted service.
5. Configures the Autofac container, registering various types.

## Configuration
- Loads configuration from "appsettings.json".
- Configures the following options:
  - `WorkerOptions`
  - `AgentProcessingOptions`
  - `OpenAIOptions`

## Service Configuration
- Adds MediatR services, registering them from the assembly containing the `Program` class.
- Adds `Worker` as a hosted service.

## Autofac Container Configuration
- Registers `ProjectFileLoader` as a singleton.
- Registers `ProjectFileHandler` as a singleton and as an `IRequestHandler<ReadProjectFilesRequest>`.
- Registers `AgentFactory` as a singleton and as `IAgentFactory`.
- Registers `AnalysisAgent` as a transient service and as an `IRequestHandler<FileProcessingRequest, ICollection<IMessage>>`.
- Registers `DocumentationHandler` as a singleton and as an `IRequestHandler<FileProcessingRequest, ICollection<IMessage>>`.

## Notes
- The application uses Autofac for dependency injection, allowing for more advanced DI scenarios.
- MediatR is configured, indicating the use of the mediator pattern for handling requests and notifications.
- The use of `IHostBuilder` suggests this is an ASP.NET Core or similar host-based application.
- Error handling in the `Main` method ensures that any unhandled exceptions are logged before the application terminates.