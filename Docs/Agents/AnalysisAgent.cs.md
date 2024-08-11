# AnalysisAgent.cs

## Overview
`AnalysisAgent` is a class responsible for analyzing file contents using AI-assisted techniques. It implements the `IRequestHandler<FileProcessingRequest, ICollection<IMessage>>` interface, indicating its role in handling file processing requests. This class is part of the `MDLabs.CriticalSqlAnalysis.Agents` namespace.

## Class Definition
```csharp
public class AnalysisAgent : IRequestHandler<FileProcessingRequest, ICollection<IMessage>>
```

## Dependencies
- `System.Diagnostics`
- `AutoGen`
- `AutoGen.Core`
- `MDLabs.CriticalSqlAnalysis.Agents.Factories`
- `MDLabs.CriticalSqlAnalysis.Handlers`
- `MDLabs.CriticalSqlAnalysis.Handlers.Requests`
- `MDLabs.CriticalSqlAnalysis.Models.Options`
- `MediatR`
- `Microsoft.Extensions.Options`

## Fields
- `private readonly MiddlewareAgent<ConversableAgent> assistant`: The assistant agent with middleware.
- `private readonly MiddlewareAgent<UserProxyAgent> user`: The user agent with middleware.
- `private readonly IOptions<AgentProcessingOptions> agentOptions`: The options for agent processing.
- `private readonly IOptions<OpenAIOptions> openAIOptions`: The options for OpenAI configuration.
- `private readonly string systemPromptPath`: The path to the system prompt file.
- `private readonly SemaphoreSlim _initializationSemaphore`: Semaphore to ensure thread-safe initialization.
- `private bool _isInitialized`: Indicates whether the agent is initialized.

## Constructor
```csharp
public AnalysisAgent(IOptions<AgentProcessingOptions> agentOptions, IOptions<OpenAIOptions> openAIOptions, IAgentFactory agentFactory)
```
Initializes a new instance of the `AnalysisAgent` class with the specified options and agent factory. It sets up the OpenAI configuration and creates the user and assistant agents.

## Methods

### Handle
```csharp
public async Task<ICollection<IMessage>> Handle(FileProcessingRequest request, CancellationToken cancellationToken)
```
Handles a file processing request by analyzing the file contents and returning a collection of messages.

### AnalyzeFileContentsAsync
```csharp
public async Task<ICollection<IMessage>> AnalyzeFileContentsAsync(FileContents fileContents, CancellationToken cancellationToken)
```
Analyzes the given file contents asynchronously using the configured agents.

### EnsureInitializedAsync
```csharp
private async Task EnsureInitializedAsync(CancellationToken cancellationToken)
```
Ensures the agent is initialized asynchronously, using a semaphore for thread safety.

## Key Features
- Uses OpenAI's GPT-3.5 model for AI-assisted analysis.
- Implements asynchronous initialization and file content analysis.
- Utilizes prompt templates for system and user instructions.
- Processes and filters agent responses.
- Implements thread-safe initialization using a semaphore.

## Error Handling
Implements try-catch blocks for error handling and logging using `Debug.WriteLine`.

## Logging
Extensive logging is implemented using `Debug.WriteLine` to track the analysis process and any errors encountered.

## Notes
- The class uses the Options pattern for configuration, injecting `IOptions<AgentProcessingOptions>` and `IOptions<OpenAIOptions>`.
- It uses a factory pattern (`IAgentFactory`) to create the user and assistant agents.
- The system prompt is loaded from a file specified by `systemPromptPath`.
- The class implements lazy initialization to ensure the agent is properly set up before use.
- Responses from the AI are filtered to remove any termination signals and non-assistant messages.