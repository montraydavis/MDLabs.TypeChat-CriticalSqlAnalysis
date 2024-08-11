using System.Diagnostics;
using MDLabs.CriticalSqlAnalysis.Handlers.Requests;
using MDLabs.CriticalSqlAnalysis.Loaders;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MDLabs.CriticalSqlAnalysis.Handlers
{
    /// <summary>
    /// Abstract base class for processing files in a project.
    /// </summary>
    public abstract class FileProcessorBase : IRequestHandler<ReadProjectFilesRequest>
    {
        /// <summary>
        /// The mediator for sending and receiving messages.
        /// </summary>
        private readonly IMediator _mediator;

        /// <summary>
        /// The loader for project files.
        /// </summary>
        private readonly ProjectFileLoader _projectFileLoader;

        /// <summary>
        /// The logger for logging information.
        /// </summary>
        private readonly ILogger<Worker> _logger;

        /// <summary>
        /// The application lifetime manager.
        /// </summary>
        private readonly IHostApplicationLifetime _lifetime;

        /// <summary>
        /// Gets the mediator for sending and receiving messages.
        /// </summary>
        protected IMediator mediator => _mediator;

        /// <summary>
        /// Gets the loader for project files.
        /// </summary>
        protected ProjectFileLoader projectFileLoader => _projectFileLoader;

        /// <summary>
        /// Gets the logger for logging information.
        /// </summary>
        protected ILogger<Worker> logger => _logger;

        /// <summary>
        /// Gets the application lifetime manager.
        /// </summary>
        protected IHostApplicationLifetime hostApplicationLifetime => _lifetime;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileProcessorBase"/> class.
        /// </summary>
        /// <param name="mediator">The mediator for sending and receiving messages.</param>
        /// <param name="projectFileLoader">The loader for project files.</param>
        /// <param name="logger">The logger for logging information.</param>
        /// <param name="lifetime">The application lifetime manager.</param>
        protected FileProcessorBase(
            IMediator mediator,
            ProjectFileLoader projectFileLoader,
            ILogger<Worker> logger,
            IHostApplicationLifetime lifetime)
        {
            _mediator = mediator;
            _projectFileLoader = projectFileLoader;
            _logger = logger;
            _lifetime = lifetime;
        }

        /// <summary>
        /// Handles the request to read project files.
        /// </summary>
        /// <param name="request">The request to read project files.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        public async Task Handle(ReadProjectFilesRequest request, CancellationToken cancellationToken)
        {
            Debug.WriteLine("Handle method started.");

            try
            {
                Debug.WriteLine($"Loading project files from path: {request.ProjectPath}");
                var projectFiles = this.projectFileLoader.LoadFiles(request.ProjectPath, new[] { ".sql" });
                var taskList = new List<Task>();

                Debug.WriteLine("Iterating over project files...");
                await foreach (var file in projectFiles)
                {
                    Debug.WriteLine($"Processing file: {file.FileName}");
                    taskList.Add(ProcessFileAsync(file, cancellationToken));
                }

                Debug.WriteLine("Waiting for all tasks to complete...");
                await Task.WhenAll(taskList);
                Debug.WriteLine("All tasks completed.");

                logger.LogInformation("Stopping application...");
                hostApplicationLifetime.StopApplication();
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
        /// Processes a file asynchronously.
        /// </summary>
        /// <param name="fileContents">The contents of the file to process.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        protected abstract Task ProcessFileAsync(FileContents fileContents, CancellationToken cancellationToken);
    }
}