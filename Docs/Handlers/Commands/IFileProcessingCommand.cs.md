The next C# source file is IFileProcessingCommand.cs. This file doesn't have existing documentation, so let's create a new markdown file for it.

# IFileProcessingCommand.cs

## Overview
`IFileProcessingCommand` is an interface that defines the contract for a file processing command in the Critical SQL Analysis Tool. It extends the `IRequest` interface from the MediatR library.

## Interface Definition
```csharp
public interface IFileProcessingCommand : IRequest
```

## Dependencies
- `MediatR`

## Properties

### FileContents
```csharp
FileContents FileContents { get; }
```
Defines a property to get the contents of the file to be processed.

## Notes
- This interface is part of the Command pattern and Mediator pattern implementation in the application.
- By extending `IRequest`, it allows the command to be handled by MediatR's pipeline.
- The interface doesn't specify a return type for `IRequest`, suggesting that handlers of this command don't return a response.
- Implementations of this interface should provide the file contents that need to be processed.