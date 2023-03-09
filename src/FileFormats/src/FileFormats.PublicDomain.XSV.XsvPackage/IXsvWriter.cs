namespace BrainVision.Lab.FileFormats.PublicDomain.XSV;

public interface IXsvWriter<T> : IAsyncDisposable where T : IConvertible
{
    Task WriteHeaderAsync(IReadOnlyCollection<string> headerRow);

    /// <exception cref="InvalidOperationException">Thrown if at least one of the <paramref name="rows"/> contains incorrect number of items.</exception>
    Task WriteAsync(IReadOnlyCollection<IReadOnlyCollection<T>> rows);
}
