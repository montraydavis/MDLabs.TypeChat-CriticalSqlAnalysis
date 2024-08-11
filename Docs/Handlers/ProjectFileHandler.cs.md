# ProjectFileHandler.cs

## Overview
`ProjectFileHandler` is a class responsible for processing project files. It inherits from `FileProcessorBase` and implements `IRequestHandler<ReadProjectFilesRequest>`, indicating its role in handling requests to read and process project files. This class is part of the `MDLabs.CriticalSqlAnalysis.Handlers` namespace.

## Class Definition
```csharp
public class ProjectFileHandler : FileProcessorBase, IRequestHandler<ReadProjectFilesRequest>
```

## Dependencies
- `System.Diagnostics`
- `MDLabs.CriticalSqlAnalysis.Handlers.Commands`
- `MDLabs.CriticalSqlAnalysis.Handlers.Requests`
- `MDLabs.CriticalSqlAnalysis.Loaders`
- `MediatR`
- `Microsoft.Extensions.Hosting`
- `Microsoft.Extensions.Logging`

## Constructor
```csharp
public ProjectFileHandler(IMediator mediator, ProjectFileLoader projectFileLoader, ILogger<Worker> logger, IHostApplicationLifetime lifetime)
    : base(mediator, projectFileLoader, logger, lifetime)
```
Initializes a new instance of the `ProjectFileHandler` class with the specified dependencies. It calls the base constructor of `FileProcessorBase`.

## Methods

### Handle
```csharp
public async Task Handle(ReadProjectFilesRequest request, CancellationToken cancellationToken)
```
This method is responsible for handling the processing of project files:

1. Loads project files using the `ProjectFileLoader`.
2. Iterates over the loaded files asynchronously.
3. Processes each file by calling `ProcessFileAsync`.
4. Waits for all processing tasks to complete.
5. Stops the application after processing is complete.
6. Implements error handling and logging.

### ProcessFileAsync
```csharp
protected override async Task ProcessFileAsync(FileContents fileContents, CancellationToken cancellationToken)
```
This protected method processes a single file:

1. Creates a `FileProcessingCommand` with the file contents.
2. Sends a `FileProcessingRequest` to the mediator for processing.

## Error Handling
The `Handle` method uses a try-catch-finally block to handle exceptions and ensure proper logging. Any exceptions are logged and re-thrown.

## Asynchronous Processing
The handler uses `Task.WhenAll` to process multiple files concurrently, improving performance for large projects.

## Logging
Extensive logging is implemented using both `Debug.WriteLine` and the injected `ILogger` to track the execution flow and aid in debugging.

## Application Lifecycle
The handler interacts with the application lifecycle, stopping the application after processing is complete using `_hostApplicationLifetime.StopApplication()`.

## Notes
- The class inherits from `FileProcessorBase`, which likely provides common functionality for file processing.
- It uses the Mediator pattern for handling requests and sending file processing commands.
- The handler is designed to work with SQL files specifically, as indicated by the file extension filter `.sql` in the base class.
- Concurrent processing of files is implemented, which can significantly improve performance for large projects.
- The application is designed to stop after all files have been processed, indicating it's meant for batch processing rather than continuous operation.