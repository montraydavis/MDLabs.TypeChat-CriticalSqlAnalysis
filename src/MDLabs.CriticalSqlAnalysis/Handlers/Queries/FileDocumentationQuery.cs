// IFileProcessingCommand.cs
using MDLabs.CriticalSqlAnalysis.Handlers;

/// <summary>
/// Represents a query for file documentation.
/// </summary>
public class FileDocumentationQuery : IFileDocumentationQuery
{
    /// <summary>
    /// Gets the contents of the file to be documented.
    /// </summary>
    public FileContents FileContents { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="FileDocumentationQuery"/> class.
    /// </summary>
    /// <param name="fileContents">The contents of the file to be documented.</param>
    public FileDocumentationQuery(FileContents fileContents)
    {
        FileContents = fileContents;
    }
}