// DocumentationHandler.cs
using System.Diagnostics;
using AutoGen.Core;
using MDLabs.CriticalSqlAnalysis.Agents;
using MDLabs.CriticalSqlAnalysis.Handlers.Commands;
using MDLabs.CriticalSqlAnalysis.Handlers.Requests;
using MediatR;

namespace MDLabs.CriticalSqlAnalysis.Handlers
{
    /// <summary>
    /// Handles the generation of documentation based on the analysis of file contents.
    /// </summary>
    public class DocumentationHandler : IRequestHandler<FileProcessingRequest, ICollection<IMessage>>
    {

        private readonly AnalysisAgent _analysisAgent;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentationHandler"/> class.
        /// </summary>
        /// <param name="analysisAgent">The analysis agent instance.</param>
        public DocumentationHandler(AnalysisAgent analysisAgent)
        {
            _analysisAgent = analysisAgent;
        }

        /// <summary>
        /// Analyzes the contents of the file asynchronously.
        /// </summary>
        /// <param name="fileContents">The contents of the file to be analyzed.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        private async Task<ICollection<IMessage>> AnalyzeFileContentsAsync(FileContents fileContents, CancellationToken cancellationToken)
        {
            return await _analysisAgent.Handle(new FileProcessingRequest(new FileProcessingCommand(fileContents)), cancellationToken);
        }

        /// <summary>
        /// Handles the file processing request.
        /// </summary>
        /// <param name="request">The file processing request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task<ICollection<IMessage>> Handle(FileProcessingRequest request, CancellationToken cancellationToken)
        {
            Debug.WriteLine($"Handling file documentation for: {request.FileProcessingCommand.FileContents.FileName}");

            var fileContentsAnalyzed = await AnalyzeFileContentsAsync(request.FileProcessingCommand.FileContents, cancellationToken);
            Debug.WriteLine($"File contents analyzed: {fileContentsAnalyzed}");

            var documentationResult = GenerateDocumentation(fileContentsAnalyzed);
            Debug.WriteLine($"Documentation result: {documentationResult}");

            return documentationResult;
        }


        /// <summary>
        /// Generates the documentation based on the analyzed file contents.
        /// </summary>
        /// <param name="fileContentsAnalyzed">The analyzed file contents.</param>
        /// <returns>A collection of messages representing the documentation.</returns>
        private ICollection<IMessage> GenerateDocumentation(ICollection<IMessage> fileContentsAnalyzed)
        {
            var documentationResult = new List<IMessage>();

            foreach (var message in fileContentsAnalyzed)
            {
                var messageContent = message.GetContent();
                if (!string.IsNullOrWhiteSpace(messageContent))
                {
                    documentationResult.Add(new TextMessage(Role.Assistant, messageContent));
                }
            }

            return documentationResult;
        }
    }
}