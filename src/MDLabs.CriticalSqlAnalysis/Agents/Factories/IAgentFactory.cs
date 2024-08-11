using AutoGen;
using AutoGen.Core;
using MDLabs.CriticalSqlAnalysis.Models.Options;
using Microsoft.Extensions.Options;

namespace MDLabs.CriticalSqlAnalysis.Agents.Factories
{
    /// <summary>
    /// Interface for creating different types of agents.
    /// </summary>
    public interface IAgentFactory
    {
        /// <summary>
        /// Creates a user agent with middleware.
        /// </summary>
        /// <returns>A <see cref="UserProxyAgent"/> instance.</returns>
        UserProxyAgent CreateUserAgent();

        /// <summary>
        /// Creates an analysis agent with middleware and specific configuration.
        /// </summary>
        /// <param name="systemPrompt">The system prompt for the analysis agent.</param>
        /// <param name="options">The processing options for the agent.</param>
        /// <returns>A <see cref="MiddlewareAgent{ConversableAgent}"/> instance.</returns>
        MiddlewareAgent<ConversableAgent> CreateAnalysisAgent(string systemPrompt, IOptions<AgentProcessingOptions> options);
    }
}

