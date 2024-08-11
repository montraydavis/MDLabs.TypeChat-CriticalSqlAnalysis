# OpenAIOptions.cs

## Overview
`OpenAIOptions` is a class that represents the configuration options for OpenAI integration in the Critical SQL Analysis Tool.

## Class Definition
```csharp
public class OpenAIOptions
```

## Dependencies
- `System`

## Properties

### ApiKey
```csharp
public string ApiKey { get; set; }
```
Gets or sets the OpenAI API key.

## Notes
- This class is used to store the OpenAI API key, which is crucial for authenticating requests to the OpenAI service.
- The `ApiKey` property is likely to be populated from a secure configuration source, such as environment variables or a secrets manager.
- The class is part of the `MDLabs.CriticalSqlAnalysis.Models.Options` namespace, indicating its role in configuration within the larger project structure.
- Care should be taken to ensure that the API key is handled securely and not exposed in logs or error messages.
- This class may be used in conjunction with dependency injection to provide the OpenAI configuration throughout the application.