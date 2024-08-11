using System.Diagnostics;
using System.Runtime.CompilerServices;
using MDLabs.CriticalSqlAnalysis.Handlers;

namespace MDLabs.CriticalSqlAnalysis.Loaders;

/// <summary>
/// Represents a loader for project files.
/// </summary>
public class ProjectFileLoader
{
    /// <summary>
    /// Loads files from the specified project directory.
    /// </summary>
    /// <param name="projectDirectory">The project directory.</param>
    /// <param name="allowedExtensions">The allowed file extensions.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An asynchronous enumerable of file contents.</returns>
    public async IAsyncEnumerable<FileContents> LoadFiles(string projectDirectory, string[] allowedExtensions, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(projectDirectory);

        if (!Directory.Exists(projectDirectory))
        {
            throw new DirectoryNotFoundException($"Project directory not found: {projectDirectory}");
        }

        Debug.WriteLine($"Loading files from directory: {projectDirectory}");
        var files = Directory.EnumerateFiles(projectDirectory, "*.*", SearchOption.AllDirectories).ToList();
        Debug.WriteLine($"Total files found: {files.Count}");

        foreach (var file in files)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var fileExtension = Path.GetExtension(file);
            if (!allowedExtensions.Any(e=>file.EndsWith(e, StringComparison.CurrentCultureIgnoreCase)))
            {
                Debug.WriteLine($"Skipping file with unsupported extension: {file}");
                continue;
            }

            Debug.WriteLine($"Processing file: {file}");
            FileContents? fileContents = null;

            try
            {
                var fileName = Path.GetFileName(file);
                var filePath = Path.GetDirectoryName(file) ?? throw new InvalidOperationException($"Unable to get directory name for file: {file}");
                var content = await File.ReadAllTextAsync(file, cancellationToken).ConfigureAwait(false);

                fileContents = new FileContents(fileName, filePath, content);
                Debug.WriteLine($"File processed successfully: {file}");
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                Debug.WriteLine($"Error processing file {file}: {ex.Message}");
            }

            if (fileContents != null)
            {
                yield return fileContents;
            }
        }
    }
}
