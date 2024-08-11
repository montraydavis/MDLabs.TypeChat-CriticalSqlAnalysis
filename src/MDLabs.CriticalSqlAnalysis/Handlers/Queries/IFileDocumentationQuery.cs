// IFileProcessingCommand.cs
using AutoGen.Core;
using MDLabs.CriticalSqlAnalysis.Handlers;
using MediatR;

/// <summary>
/// Represents a query for file documentation.
/// </summary>
public interface IFileDocumentationQuery : IRequest<ICollection<IMessage>>
{
    /// <summary>
    /// Gets the contents of the file to be documented.
    /// </summary>
    FileContents FileContents { get; }
}