# IAgentFactory.cs

## Overview
`IAgentFactory` is an interface that defines the contract for creating different types of agents used in the Critical SQL Analysis Tool.

## Interface Definition
```csharp
public interface IAgentFactory
```

## Dependencies
- `AutoGen`
- `AutoGen.Core`
- `MDLabs.CriticalSqlAnalysis.Models.Options`
- `Microsoft.Extensions.Options`

## Methods

### CreateUserAgent
```csharp
MiddlewareAgent<UserProxyAgent> CreateUserAgent();
```
Defines a method to create a user agent with middleware.

### CreateAnalysisAgent
```csharp
MiddlewareAgent<ConversableAgent> CreateAnalysisAgent(string systemPrompt, IOptions<AgentProcessingOptions> options);
```
Defines a method to create an analysis agent with middleware and specific configuration.

## Notes
- This interface is likely implemented by concrete factory classes to provide different implementations of agent creation.
- The use of `MiddlewareAgent<T>` suggests that the created agents will have additional processing capabilities.
- The `CreateAnalysisAgent` method takes a system prompt and options, allowing for flexible configuration of the analysis agent.