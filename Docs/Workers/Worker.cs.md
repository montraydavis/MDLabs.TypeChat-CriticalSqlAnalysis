# Worker.cs

## Overview
`Worker` is a background service class that processes project files. It inherits from `BackgroundService`, indicating it's designed to run as a long-running service within the application. This class is part of the `MDLabs.CriticalSqlAnalysis` namespace.

## Class Definition
```csharp
public class Worker : BackgroundService
```

## Dependencies
- `System.Diagnostics`
- `MDLabs.CriticalSqlAnalysis.Handlers.Requests`
- `MDLabs.CriticalSqlAnalysis.Models.Options`
- `MediatR`
- `Microsoft.Extensions.Hosting`
- `Microsoft.Extensions.Logging`
- `Microsoft.Extensions.Options`

## Fields
- `private readonly IMediator _mediator`: The mediator instance used for sending requests.
- `private readonly ILogger<Worker> _logger`: The logger instance for logging information and errors.
- `private readonly WorkerOptions _options`: The options for configuring the worker.

## Constructor
```csharp
public Worker(IMediator mediator, ILogger<Worker> logger, IOptions<WorkerOptions> options)
```
Initializes a new instance of the `Worker` class with the specified mediator, logger, and options. Throws `ArgumentNullException` if any parameter is null.

## Properties
- `private IMediator mediator => _mediator`: Gets the mediator instance.

## Methods

### ExecuteAsync
```csharp
protected override async Task ExecuteAsync(CancellationToken stoppingToken)
```
This method is the core of the background service:
1. It's called when the service starts and runs continuously until cancellation is requested.
2. Processes SQL scripts asynchronously at regular intervals (every hour).
3. Implements error handling and logging.

### ProcessSqlScriptsAsync
```csharp
private async Task ProcessSqlScriptsAsync(CancellationToken stoppingToken)
```
This private method:
1. Sends a `ReadProjectFilesRequest` to the mediator with the path from `_options.SqlScriptsPath`.
2. Implements error handling and logging.

## Error Handling
The class uses try-catch blocks to handle exceptions in both `ExecuteAsync` and `ProcessSqlScriptsAsync` methods. Exceptions are logged using the injected `ILogger<Worker>` instance.

## Logging
Extensive logging is implemented using both `Debug.WriteLine` and the injected `ILogger<Worker>` to track the execution flow and any errors encountered.

## Notes
- The class uses the Options pattern for configuration, injecting `IOptions<WorkerOptions>` to get the SQL scripts path.
- The worker runs continuously, processing SQL scripts every hour unless cancellation is requested.
- The use of `BackgroundService` suggests this worker is designed to run throughout the application's lifetime.
- The class leverages dependency injection for its dependencies (IMediator, ILogger, and IOptions<WorkerOptions>).