using System.Diagnostics;
using AutoGen;
using AutoGen.Core;
using MDLabs.CriticalSqlAnalysis.Agents.Factories;
using MDLabs.CriticalSqlAnalysis.Handlers;
using MDLabs.CriticalSqlAnalysis.Handlers.Requests;
using MDLabs.CriticalSqlAnalysis.Models.Options;
using MediatR;
using Microsoft.Extensions.Options;

namespace MDLabs.CriticalSqlAnalysis.Agents
{

    /// <summary>
    /// Handles file processing requests and analyzes file contents using agents.
    /// </summary>
    public class AnalysisAgent : IRequestHandler<FileProcessingRequest, ICollection<IMessage>>
    {
        /// <summary>
        /// The assistant agent with middleware.
        /// </summary>
        private readonly MiddlewareAgent<ConversableAgent> _assistantAgent;

        /// <summary>
        /// The user agent with middleware.
        /// </summary>
        private readonly UserProxyAgent _userAgent;

        /// <summary>
        /// The options for agent processing.
        /// </summary>
        private readonly IOptions<AgentProcessingOptions> _agentOptions;

        /// <summary>
        /// The options for OpenAI configuration.
        /// </summary>
        private readonly IOptions<OpenAIOptions> _openAIOptions;

        /// <summary>
        /// The path to the system prompt file.
        /// </summary>
        private readonly string _systemPromptPath;

        /// <summary>
        /// Semaphore to ensure thread-safe initialization.
        /// </summary>
        private readonly SemaphoreSlim _initializationSemaphore = new SemaphoreSlim(1, 1);

        /// <summary>
        /// Indicates whether the agent is initialized.
        /// </summary>
        private bool _isInitialized = false;


        /// <summary>
        /// Initializes a new instance of the <see cref="AnalysisAgent"/> class.
        /// </summary>
        /// <param name="agentOptions">The options for agent processing.</param>
        /// <param name="openAIOptions">The options for OpenAI configuration.</param>
        /// <param name="agentFactory">The factory for creating agents.</param>
        /// <exception cref="ArgumentNullException">Thrown when agentOptions or openAIOptions is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the OpenAI API key is not found in configuration.</exception>
        public AnalysisAgent(IOptions<AgentProcessingOptions> agentOptions, IOptions<OpenAIOptions> openAIOptions, IAgentFactory agentFactory)
        {
            Debug.WriteLine("AnalysisAgent constructor started.");
            _agentOptions = agentOptions ?? throw new ArgumentNullException(nameof(agentOptions));
            _openAIOptions = openAIOptions ?? throw new ArgumentNullException(nameof(openAIOptions));
            _systemPromptPath = "./Resources/Prompts/Documentation/Instruct-Metrics.system.md";

            var sysPrompt = File.ReadAllText(_systemPromptPath);
            if (string.IsNullOrEmpty(_openAIOptions.Value.ApiKey))
            {
                throw new InvalidOperationException("OpenAI API key not found in configuration.");
            }

            Environment.SetEnvironmentVariable("OPENAI_API_KEY", _openAIOptions.Value.ApiKey);

            _userAgent = agentFactory.CreateUserAgent();
            _assistantAgent = agentFactory.CreateAnalysisAgent(systemPrompt: sysPrompt, options: _agentOptions);

            Debug.WriteLine("AnalysisAgent constructor completed.");
        }

        private string PrepareFileContents(FileContents fileContents)
        {
            return $"-- File Name: {fileContents.FileName}\n\n{fileContents.Content}";
        }

        private async Task<string> LoadUserPromptAsync(CancellationToken cancellationToken)
        {
            return await File.ReadAllTextAsync("./Resources/Prompts/Documentation/Instruct-Metrics.user.md", cancellationToken);
        }

        private ICollection<IMessage> ProcessAgentResponse(ICollection<IMessage> response)
        {
            var responseMessages = new List<IMessage>();
            foreach (var message in response)
            {
                Debug.WriteLine($"Processing message: {message.GetContent()}");
                if (message.GetContent()?.Contains("<TERMINATE>") == true)
                {
                    Debug.WriteLine("Terminate signal found in message. Breaking the loop.");
                    break;
                }
                if (message.GetRole() == Role.Assistant && message.From != "user")
                {
                    Debug.WriteLine("Adding assistant message to response list.");
                    responseMessages.Add(message);
                }
            }
            return responseMessages;
        }

        ///// <summary>
        ///// Analyzes the contents of a file asynchronously.
        ///// </summary>
        ///// <param name="fileContents">The contents of the file to analyze.</param>
        ///// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        ///// <returns>A collection of messages resulting from the analysis.</returns>
        //public async Task<ICollection<IMessage>> AnalyzeFileContentsAsync(FileContents fileContents, CancellationToken cancellationToken)
        //{
        //    Debug.WriteLine("AnalyzeFileContentsAsync method started.");
        //    try
        //    {
        //        await EnsureInitializedAsync(cancellationToken);

        //        var sqlContents = $"-- File Name: {fileContents.FileName}\n\n{fileContents.Content}";
        //        Debug.WriteLine($"SQL contents prepared: {sqlContents}");

        //        var userPrompt = await File.ReadAllTextAsync("./Resources/Prompts/Documentation/Instruct-Metrics.user.md", cancellationToken);
        //        userPrompt = userPrompt.Replace("@sql", sqlContents);
        //        Debug.WriteLine("User prompt prepared.");

        //        Debug.WriteLine("Sending prompts to assistant.");
        //        var response = await this._userAgent.SendAsync(this._assistantAgent, new List<IMessage>(), 10, cancellationToken);
        //        Debug.WriteLine("Response received from assistant.");

        //        var responseList = new List<IMessage>();

        //        Debug.WriteLine("Processing response messages.");
        //        foreach (var message in response)
        //        {
        //            Debug.WriteLine($"Processing message: {message.GetContent()}");

        //            if (message.GetContent()?.Contains("<TERMINATE>") == true)
        //            {
        //                Debug.WriteLine("Terminate signal found in message. Breaking the loop.");
        //                break;
        //            }

        //            if (message.GetRole() == Role.Assistant && message.From != "user")
        //            {
        //                Debug.WriteLine("Adding assistant message to response list.");
        //                responseList.Add(message);
        //            }
        //        }

        //        Debug.WriteLine("Response processing completed.");
        //        return responseList;
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine($"Exception occurred: {ex.Message}");
        //        throw;
        //    }
        //    finally
        //    {
        //        Debug.WriteLine("AnalyzeFileContentsAsync method completed.");
        //    }
        //}

        /// <summary>
        /// Handles a file processing request asynchronously.
        /// </summary>
        /// <param name="request">The file processing request.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A collection of messages resulting from the handling of the request.</returns>
        public async Task<ICollection<IMessage>> Handle(FileProcessingRequest request, CancellationToken cancellationToken)
        {
            Debug.WriteLine("Handle method started.");
            try
            {
                await EnsureInitializedAsync(cancellationToken);
                var fileContents = request.FileProcessingCommand.FileContents;
                var sqlContents = PrepareFileContents(fileContents);
                var tmpPrompt = await LoadUserPromptAsync(cancellationToken);
                var userPrompt = tmpPrompt.Replace("@sql", sqlContents);

                Debug.WriteLine("Sending prompts to assistant.");
                var response = await this._userAgent.SendAsync(this._assistantAgent, userPrompt, new List<IMessage>(), 10, cancellationToken);
                Debug.WriteLine("Response received from assistant.");

                var responseMessages = ProcessAgentResponse(response.ToArray());

                Debug.WriteLine("Response processing completed.");
                return responseMessages;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception occurred: {ex.Message}");
                throw;
            }
            finally
            {
                Debug.WriteLine("Handle method completed.");
            }
        }

        /// <summary>
        /// Ensures the agent is initialized asynchronously.
        /// </summary>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        private async Task EnsureInitializedAsync(CancellationToken cancellationToken)
        {
            if (_isInitialized) return;

            await _initializationSemaphore.WaitAsync(cancellationToken);
            try
            {
                if (_isInitialized) return;

                Debug.WriteLine($"Initializing {nameof(AnalysisAgent)}...");
                var systemPrompt = await File.ReadAllTextAsync(_systemPromptPath, cancellationToken);
                await this._userAgent.InitiateChatAsync(this._assistantAgent, systemPrompt, 3);
                _isInitialized = true;
                Debug.WriteLine($"Initialized {nameof(AnalysisAgent)}.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception occurred: {ex.Message}");
                throw;
            }
            finally
            {
                _initializationSemaphore.Release();
            }
        }


    }


}