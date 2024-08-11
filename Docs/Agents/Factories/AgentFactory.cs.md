# AgentFactory.cs

## Overview
`AgentFactory` is a class responsible for creating different types of agents used in the Critical SQL Analysis Tool. It implements the `IAgentFactory` interface, providing methods to create user agents and analysis agents.

## Class Definition
```csharp
public class AgentFactory : IAgentFactory
```

## Dependencies
- `AutoGen`
- `AutoGen.Core`
- `AutoGen.OpenAI`
- `MDLabs.CriticalSqlAnalysis.Models.Options`
- `Microsoft.Extensions.Options`
- `System.Diagnostics`

## Methods

### CreateUserAgent
```csharp
public MiddlewareAgent<UserProxyAgent> CreateUserAgent()
```
Creates and returns a user agent with middleware:
1. Creates a `UserProxyAgent` with default settings.
2. Wraps it in a `MiddlewareAgent` for additional processing.
3. Configures middleware to log invocations.
4. Returns the configured agent.

### CreateAnalysisAgent
```csharp
public MiddlewareAgent<ConversableAgent> CreateAnalysisAgent(string systemPrompt, IOptions<AgentProcessingOptions> options)
```
Creates and returns an analysis agent with middleware and OpenAI integration:
1. Creates a `UserProxyAgent` for analysis.
2. Wraps it in a `MiddlewareAgent` for additional processing.
3. Configures middleware to log invocations.
4. Retrieves the OpenAI API key from environment variables.
5. Configures OpenAI integration with the "gpt-3.5-turbo" model.
6. Creates a `ConversableAgent` with the provided system prompt and OpenAI configuration.
7. Returns the configured agent.

## Error Handling
- Throws an `InvalidOperationException` if the OpenAI API key is not found in environment variables.

## Logging
Uses `Debug.WriteLine` for logging method invocations and important steps in agent creation.

## Notes
- The class implements the `IAgentFactory` interface, suggesting it's part of a factory pattern implementation.
- OpenAI integration is configured with specific temperature and timeout settings from the provided options.
- The OpenAI API key is expected to be set in the environment variables for security reasons.