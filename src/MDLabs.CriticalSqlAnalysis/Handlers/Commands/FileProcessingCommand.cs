using System;
namespace MDLabs.CriticalSqlAnalysis.Handlers.Commands
{
    /// <summary>
    /// Represents a command for processing a file.
    /// </summary>
    public class FileProcessingCommand : IFileProcessingCommand
    {
        /// <summary>
        /// Gets the contents of the file to be processed.
        /// </summary>
        public FileContents FileContents { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileProcessingCommand"/> class.
        /// </summary>
        /// <param name="fileContents">The contents of the file to be processed.</param>
        public FileProcessingCommand(FileContents fileContents)
        {
            FileContents = fileContents;
        }
    }
}

