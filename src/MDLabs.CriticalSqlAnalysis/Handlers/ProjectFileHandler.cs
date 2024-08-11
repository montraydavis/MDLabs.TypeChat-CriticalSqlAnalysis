using System.Diagnostics;
using AutoGen.Core;
using MDLabs.CriticalSqlAnalysis.Handlers.Commands;
using MDLabs.CriticalSqlAnalysis.Handlers.Requests;
using MDLabs.CriticalSqlAnalysis.Loaders;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MDLabs.CriticalSqlAnalysis.Handlers
{
    /// <summary>
    /// Responsible for processing project files.
    /// </summary>
    public class ProjectFileHandler : FileProcessorBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Gets the mediator instance.
        /// </summary>
        private IMediator mediator => _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectFileHandler"/> class.
        /// </summary>
        /// <param name="mediator">The mediator instance.</param>
        /// <param name="projectFileLoader">The project file loader instance.</param>
        /// <param name="logger">The logger instance.</param>
        /// <param name="lifetime">The host application lifetime instance.</param>
        public ProjectFileHandler(
            IMediator mediator,
            ProjectFileLoader projectFileLoader,
            ILogger<Worker> logger,
            IHostApplicationLifetime lifetime)
        : base(mediator, projectFileLoader, logger, lifetime)
        {
            _mediator = mediator;
        }

        private async Task SendFileProcessingRequestAsync(FileContents fileContents, CancellationToken cancellationToken)
        {
            await _mediator.Send(new FileProcessingRequest(new FileProcessingCommand(fileContents)), cancellationToken);
        }

        // private async Task<ICollection<IMessage>> SendFileDocumentationQueryAsync(FileContents fileContents, CancellationToken cancellationToken)
        // {
        //     var fileDocumentationQuery = new FileDocumentationQuery(fileContents);
        //     return await _mediator.Send(fileDocumentationQuery, cancellationToken);
        // }

        private void ProcessDocumentationResult(ICollection<IMessage> documentationResult)
        {
            // Process the documentation result
            Debug.WriteLine($"Documentation result: {documentationResult}");

            // Save the documentation result to a file or perform any other necessary actions
        }

        protected override async Task ProcessFileAsync(FileContents fileContents, CancellationToken cancellationToken)
        {
            Debug.WriteLine($"Processing file: {fileContents.FileName}");

            await SendFileProcessingRequestAsync(fileContents, cancellationToken);
            // var documentationResult = await SendFileDocumentationQueryAsync(fileContents, cancellationToken);

            //ProcessDocumentationResult(documentationResult);
        }
    }
}