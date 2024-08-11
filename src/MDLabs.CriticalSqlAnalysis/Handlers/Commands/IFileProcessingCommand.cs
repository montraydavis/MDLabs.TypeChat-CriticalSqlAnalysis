using System;
using MediatR;

namespace MDLabs.CriticalSqlAnalysis.Handlers.Commands
{

    /// <summary>
    /// Represents a command for processing a file.
    /// </summary>
    public interface IFileProcessingCommand : IRequest
    {
        /// <summary>
        /// Gets the contents of the file to be processed.
        /// </summary>
        FileContents FileContents { get; }
    }
}

