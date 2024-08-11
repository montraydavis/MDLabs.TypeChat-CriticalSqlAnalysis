# AgentUtilities.cs

## Overview
`AgentUtilities` is a utility class that provides methods for creating and configuring agents used in the analysis process.

## Class Definition
```csharp
public class AgentUtilities
```

## Dependencies
- `System`
- `System.Diagnostics`
- `AutoGen`
- `AutoGen.Core`
- `AutoGen.OpenAI`
- `MDLabs.CriticalSqlAnalysis.Agents`

## Methods

### CreateInstance
```csharp
public static AgentUtilities CreateInstance()
```
Creates and returns a new instance of `AgentUtilities`.

### CreateUserAgent
```csharp
public static MiddlewareAgent<UserProxyAgent> CreateUserAgent()
```
Creates and configures a user agent with middleware for message handling.

### CreateAnalysisAgent
```csharp
internal static MiddlewareAgent<ConversableAgent> CreateAnalysisAgent(string systemPrompt)
```
Creates and configures an analysis agent with middleware for message handling and OpenAI integration.

## Key Features
- Uses middleware to intercept and process messages for both user and analysis agents.
- Configures OpenAI integration for the analysis agent.
- Implements logging using `Debug.WriteLine`.

## OpenAI Configuration
- Retrieves the OpenAI API key from environment variables.
- Uses the "gpt-3.5-turbo" model for the analysis agent.

## Notes
- The class is designed with a protected constructor, suggesting it's not meant to be instantiated directly.
- It uses the AutoGen library for agent creation and configuration.
- The OpenAI API key is expected to be set in the environment variables for security.
- The analysis agent is configured with specific temperature and timeout settings for consistent behavior.