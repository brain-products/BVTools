namespace BrainVision.Lab.FileFormats.PublicDomain.XSV;

public interface IXsvReader<T> : IDisposable where T : IConvertible
{
    IReadOnlyList<string>? HeaderRow { get; }

    /// <exception cref="InvalidXsvFileFormatException">Thrown if at least one row contains incorrect number of items or if an item cannot not be converted to T (has invalid format).</exception>
    Task<IList<T[]>> ReadToEndAsync();

    /// <exception cref="InvalidXsvFileFormatException">Thrown if at least one row contains incorrect number of items or if an item cannot not be converted to T (has invalid format).</exception>
    IAsyncEnumerable<T[]> EnumerateRowsAsync();
}
