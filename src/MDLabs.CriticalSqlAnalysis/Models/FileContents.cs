using MediatR;

namespace MDLabs.CriticalSqlAnalysis.Handlers;

/// <summary>
/// Represents the contents of a file.
/// </summary>
public class FileContents : IRequest
{

    /// <summary>
    /// Gets the name of the file.
    /// </summary>
    public string FileName { get; init; }

    /// <summary>
    /// Gets the path of the file.
    /// </summary>
    public string FilePath { get; init; }

    /// <summary>
    /// Gets the content of the file.
    /// </summary>
    public string Content { get; init; }


    /// <summary>
    /// Initializes a new instance of the <see cref="FileContents"/> class.
    /// </summary>
    /// <param name="fileName">The name of the file.</param>
    /// <param name="filePath">The path of the file.</param>
    /// <param name="content">The content of the file.</param>
    /// <exception cref="ArgumentNullException">Thrown if any parameter is null.</exception>
    /// <exception cref="ArgumentException">Thrown if any string parameter is empty or whitespace.</exception>
    public FileContents(string fileName, string filePath, string content)
    {
        FileName = !string.IsNullOrWhiteSpace(fileName) ? fileName : throw new ArgumentException("File name cannot be null or whitespace.", nameof(fileName));
        FilePath = !string.IsNullOrWhiteSpace(filePath) ? filePath : throw new ArgumentException("File path cannot be null or whitespace.", nameof(filePath));
        Content = content ?? throw new ArgumentNullException(nameof(content));
    }
    /// <summary>
    /// Determines whether the specified <see cref="FileContents"/> is equal to the current <see cref="FileContents"/>.
    /// </summary>
    /// <param name="other">The <see cref="FileContents"/> to compare with the current <see cref="FileContents"/>.</param>
    /// <returns><c>true</c> if the specified <see cref="FileContents"/> is equal to the current <see cref="FileContents"/>; otherwise, <c>false</c>.</returns>
    public bool Equals(FileContents? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return FileName == other.FileName && FilePath == other.FilePath && Content == other.Content;
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current <see cref="FileContents"/>.
    /// </summary>
    /// <param name="obj">The object to compare with the current <see cref="FileContents"/>.</param>
    /// <returns><c>true</c> if the specified object is equal to the current <see cref="FileContents"/>; otherwise, <c>false</c>.</returns>
    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((FileContents)obj);
    }

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current <see cref="FileContents"/>.</returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(FileName, FilePath, Content);
    }

    /// <summary>
    /// Determines whether two specified instances of <see cref="FileContents"/> are equal.
    /// </summary>
    /// <param name="left">The first <see cref="FileContents"/> to compare.</param>
    /// <param name="right">The second <see cref="FileContents"/> to compare.</param>
    /// <returns><c>true</c> if the two <see cref="FileContents"/> instances are equal; otherwise, <c>false</c>.</returns>
    public static bool operator ==(FileContents? left, FileContents? right)
    {
        return Equals(left, right);
    }

    /// <summary>
    /// Determines whether two specified instances of <see cref="FileContents"/> are not equal.
    /// </summary>
    /// <param name="left">The first <see cref="FileContents"/> to compare.</param>
    /// <param name="right">The second <see cref="FileContents"/> to compare.</param>
    /// <returns><c>true</c> if the two <see cref="FileContents"/> instances are not equal; otherwise, <c>false</c>.</returns>
    public static bool operator !=(FileContents? left, FileContents? right)
    {
        return !Equals(left, right);
    }
}