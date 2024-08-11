# FileProcessingCommand.cs

## Overview
`FileProcessingCommand` is a class that implements the `IFileProcessingCommand` interface. It represents a command for processing a file in the Critical SQL Analysis Tool.

## Class Definition
```csharp
public class FileProcessingCommand : IFileProcessingCommand
```

## Dependencies
- `MDLabs.CriticalSqlAnalysis.Handlers.Commands`

## Properties

### FileContents
```csharp
public FileContents FileContents { get; }
```
Gets the contents of the file to be processed.

## Constructor
```csharp
public FileProcessingCommand(FileContents fileContents)
```
Initializes a new instance of the `FileProcessingCommand` class with the specified file contents.

## Notes
- This class is part of the Command pattern implementation in the application.
- It encapsulates the file contents that need to be processed, allowing for separation of concerns between the command creation and execution.
- The `FileContents` property is read-only, ensuring immutability of the command once created.