# FileContents.cs

## Overview
`FileContents` is a class that represents the contents of a file. It implements the `IRequest` interface from MediatR, indicating its use in a mediator-based request/response pattern. This class is part of the `MDLabs.CriticalSqlAnalysis.Handlers` namespace.

## Class Definition
```csharp
public class FileContents : IRequest, IEquatable<FileContents>
```

## Dependencies
- `MediatR`
- `System`

## Properties
- `public string FileName { get; init; }`: Gets the name of the file.
- `public string FilePath { get; init; }`: Gets the path of the file.
- `public string Content { get; init; }`: Gets the content of the file.

## Constructor
```csharp
public FileContents(string fileName, string filePath, string content)
```
Initializes a new instance of the `FileContents` class with the specified file name, file path, and content.

## Methods

### Equals(FileContents? other)
```csharp
public bool Equals(FileContents? other)
```
Determines whether the specified `FileContents` is equal to the current `FileContents`.

### Equals(object? obj)
```csharp
public override bool Equals(object? obj)
```
Determines whether the specified object is equal to the current `FileContents`.

### GetHashCode()
```csharp
public override int GetHashCode()
```
Serves as the default hash function for `FileContents`.

### Equality Operators
```csharp
public static bool operator ==(FileContents? left, FileContents? right)
public static bool operator !=(FileContents? left, FileContents? right)
```
Determine whether two specified instances of `FileContents` are equal or not equal.

## Exception Handling
The constructor throws:
- `ArgumentException` if `fileName` or `filePath` is null, empty, or consists only of white-space characters.
- `ArgumentNullException` if `content` is null.

## Notes
- The class is immutable, with all properties being init-only setters.
- It implements `IEquatable<FileContents>` for efficient equality comparisons.
- The class overrides `Equals` and `GetHashCode` methods to provide value-based equality.
- Custom equality operators (`==` and `!=`) are provided for convenience.
- The implementation of `IRequest` without a type parameter suggests that handlers for this request type don't return a response.
- The class uses C# 9.0 features like init-only setters for properties.