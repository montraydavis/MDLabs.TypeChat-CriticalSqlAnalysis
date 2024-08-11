# WorkerOptions.cs

## Overview
`WorkerOptions` is a class that represents configuration options for the worker component in the Critical SQL Analysis Tool.

## Class Definition
```csharp
public class WorkerOptions
```

## Dependencies
- None explicitly shown in the provided code.

## Properties

### SqlScriptsPath
```csharp
public string SqlScriptsPath { get; set; }
```
Gets or sets the path to the SQL scripts.

## Notes
- This class is used to configure the worker component, specifically the path where SQL scripts are located.
- The `SqlScriptsPath` property is likely to be populated from a configuration file or environment variable.
- The class is part of the `MDLabs.CriticalSqlAnalysis.Models.Options` namespace, indicating its role in configuration within the larger project structure.
- This configuration allows for flexibility in specifying where the SQL scripts are stored, which can be useful for different deployment environments or testing scenarios.
- The class may be used with dependency injection to provide the worker configuration throughout the application.