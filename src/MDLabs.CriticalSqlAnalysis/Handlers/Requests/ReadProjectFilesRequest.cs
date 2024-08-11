using MediatR;

namespace MDLabs.CriticalSqlAnalysis.Handlers.Requests
{

    /// <summary>
    /// Represents a request to read project files.
    /// </summary>
    public class ReadProjectFilesRequest : IRequest
    {
        /// <summary>
        /// Gets the path of the project.
        /// </summary>
        public string ProjectPath { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadProjectFilesRequest"/> class.
        /// </summary>
        /// <param name="projectPath">The path of the project.</param>
        public ReadProjectFilesRequest(string projectPath)
        {
            ProjectPath = projectPath ?? throw new ArgumentNullException(nameof(projectPath));
        }
    }
}

