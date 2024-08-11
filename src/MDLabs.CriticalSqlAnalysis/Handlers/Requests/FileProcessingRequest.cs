using AutoGen.Core;
using MDLabs.CriticalSqlAnalysis.Handlers.Commands;
using MediatR;

namespace MDLabs.CriticalSqlAnalysis.Handlers.Requests
{
    /// <summary>
    /// Represents a request for processing a file.
    /// </summary>
    public class FileProcessingRequest : IRequest<ICollection<IMessage>>
    {
        /// <summary>
        /// Gets the command for processing the file.
        /// </summary>
        public IFileProcessingCommand FileProcessingCommand { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileProcessingRequest"/> class.
        /// </summary>
        /// <param name="fileProcessingCommand">The command for processing the file.</param>
        public FileProcessingRequest(IFileProcessingCommand fileProcessingCommand)
        {
            FileProcessingCommand = fileProcessingCommand;
        }
    }
}

