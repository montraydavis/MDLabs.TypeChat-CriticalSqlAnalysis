using AutoGen;
using AutoGen.Core;
using AutoGen.OpenAI;
using MDLabs.CriticalSqlAnalysis.Models.Options;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace MDLabs.CriticalSqlAnalysis.Agents.Factories
{

    /// <summary>
    /// Factory class for creating different types of agents.
    /// </summary>
    public class AgentFactory : IAgentFactory
    {
        /// <summary>
        /// Creates a user agent with middleware.
        /// </summary>
        /// <returns>A <see cref="MiddlewareAgent{AssistantAgent}"/> instance.</returns>
        public UserProxyAgent CreateUserAgent()
        {
            Debug.WriteLine("Creating user agent...");
            var userAgentProxy = new UserProxyAgent(name: nameof(UserProxyAgent));
            var userMiddlewareAgent = new MiddlewareAgent(innerAgent: userAgentProxy);

            userMiddlewareAgent.Use(async (messages, options, agent, ct) =>
            {
                Debug.WriteLine($"Middleware invoked: {nameof(AnalysisAgent)}");
                return await agent.GenerateReplyAsync(messages, options, ct);
            });

            var agent = new UserProxyAgent(
                name: "user",
                defaultReply: "If complete, respond only with '<TERMINATE>', otherwise continue generation without including any additional text.",
                humanInputMode: HumanInputMode.NEVER) ;

            //agent
            //    .Use(userMiddlewareAgent.Middlewares.First());

            Debug.WriteLine("User agent created successfully.");
            return agent;
        }

        /// <summary>
        /// Creates an analysis agent with middleware and specific configuration.
        /// </summary>
        /// <param name="systemPrompt">The system prompt for the analysis agent.</param>
        /// <param name="options">The processing options for the agent.</param>
        /// <returns>A <see cref="MiddlewareAgent{ConversableAgent}"/> instance.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the OpenAI API key is not found in environment variables.</exception>
        public MiddlewareAgent<ConversableAgent> CreateAnalysisAgent(string systemPrompt, IOptions<AgentProcessingOptions> options)
        {
            Debug.WriteLine("Creating analysis agent...");
            //var analysisAgentProxy = new UserProxyAgent(name: nameof(AnalysisAgent));

            //var analysisMiddlewareAgent = new MiddlewareAgent(innerAgent: analysisAgentProxy);

            //analysisMiddlewareAgent.Use(async (messages, agentOptions, agent, ct) =>
            //{
            //    Debug.WriteLine($"Middleware invoked: {nameof(AnalysisAgent)}");
            //    return await agent.GenerateReplyAsync(messages, agentOptions, ct);
            //});

            var openAiApiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
            if (string.IsNullOrEmpty(openAiApiKey))
            {
                throw new InvalidOperationException("OpenAI API key not found in environment variables.");
            }

            var gpt35Config = new OpenAIConfig(openAiApiKey, "gpt-3.5-turbo");

            var agent = new ConversableAgent(
                name: nameof(AnalysisAgent),
                systemMessage: systemPrompt,
                llmConfig: new ConversableAgentConfig
                {
                    Temperature = options.Value.Temperature,
                    Timeout = options.Value.Timeout,
                    ConfigList = [gpt35Config]
                },
                humanInputMode: HumanInputMode.NEVER)
                .RegisterPrintMessage();
            //.RegisterMiddleware(analysisMiddlewareAgent.Middlewares.First());

            agent.Use(async (messages, options, tagent, ct) =>
            {
                if (messages.LastOrDefault(m => m.From != "user") is TextMessage lastMessage && lastMessage.From != "user" && lastMessage.Content.Contains("<TERMINATE>"))
                {
                    return lastMessage;
                }

                return await tagent.GenerateReplyAsync(messages, options ?? new GenerateReplyOptions()
                {
                    MaxToken = 4000
                }, ct);
            });

            Debug.WriteLine($"Analysis agent created successfully with Temperature: {options.Value.Temperature}, Timeout: {options.Value.Timeout}");
            return agent;
        }
    }
}

