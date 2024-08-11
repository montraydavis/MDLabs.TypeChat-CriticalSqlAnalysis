using System;

namespace MDLabs.CriticalSqlAnalysis.Models.Options;

/// <summary>
/// Represents the processing options for the agent.
/// </summary>
public class AgentProcessingOptions
{
    /// <summary>
    /// Gets or sets the temperature for the language model.
    /// </summary>
    public float Temperature { get; set; } = 0.25f;

    /// <summary>
    /// Gets or sets the timeout for the language model in milliseconds.
    /// </summary>
    public int Timeout { get; set; } = 30000;
}
