# ProjectFileLoader.cs

## Overview
`ProjectFileLoader` is a class responsible for loading project files from a specified directory. It provides asynchronous enumeration of file contents that match allowed extensions.

## Class Definition
```csharp
public class ProjectFileLoader
```

## Dependencies
- `System.Diagnostics`
- `MDLabs.CriticalSqlAnalysis.Handlers`

## Methods

### LoadFiles
```csharp
public async IAsyncEnumerable<FileContents> LoadFiles(string projectDirectory, string[] allowedExtensions)
```

This method is responsible for loading files from the specified project directory:

1. Retrieves all files from the project directory and its subdirectories.
2. Filters files based on the allowed extensions.
3. Processes each file asynchronously:
   - Extracts file name and path.
   - Reads file content asynchronously.
   - Creates a `FileContents` object for each valid file.
4. Yields `FileContents` objects asynchronously.
5. Implements error handling and logging using `Debug.WriteLine`.

## Error Handling
The method uses a try-catch block within the file processing loop to handle exceptions for individual files. Errors are logged, and processing continues with the next file.

## Asynchronous Processing
The method uses `IAsyncEnumerable<T>` to allow for asynchronous iteration over the loaded files, which can improve performance when dealing with large numbers of files or large file sizes.

## Logging
Extensive logging is implemented using `Debug.WriteLine` to track the execution flow, file processing status, and any errors encountered.

## File Filtering
The method filters files based on the provided `allowedExtensions` array, ensuring that only files with specified extensions are processed.

## Notes
- The class uses modern C# features like `IAsyncEnumerable<T>` for asynchronous streaming of results.
- File content is read asynchronously using `File.ReadAllTextAsync`, which can help with performance for large files.
- The method is designed to be resilient, continuing to process files even if an error occurs with a single file.
- The class is part of the `MDLabs.CriticalSqlAnalysis.Loaders` namespace, indicating its role in the larger project structure.