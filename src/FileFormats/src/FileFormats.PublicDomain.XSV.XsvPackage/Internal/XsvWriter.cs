using System.Globalization;
using System.Text;
using BrainVision.Lab.FileFormats.PublicDomain.XSV.Properties;
using BrainVision.Lab.SystemExt.Internal;

namespace BrainVision.Lab.FileFormats.PublicDomain.XSV.Internal;

internal sealed class XsvWriter<T> : IXsvWriter<T> where T : IConvertible
{
    private readonly StreamWriter _stream;
    private readonly char _separator;
    private readonly StringBuilder _stringBuilder = new();
    private readonly Type _tType = typeof(T);
    private readonly Type[] _supportedTypes = { typeof(int), typeof(double), typeof(string) };
    private int _firstRowSize = -1;

    public XsvWriter(string fileName, char separator)
    {
        if (!_supportedTypes.Any(type => type == _tType))
            CommonExceptions.ThrowNotSupported($"{Resources.UnsupportedType} ({_tType})");

        _stream = new StreamWriter(fileName, false, Encoding.UTF8);
        _separator = separator;
    }

    public async ValueTask DisposeAsync() => await _stream.DisposeAsync().ConfigureAwait(false);

    public async Task WriteHeaderAsync(IReadOnlyCollection<string> header) =>
        await WriteSingleRowAsync(header).ConfigureAwait(false);

    /// <exception cref="InvalidOperationException">Thrown if <paramref name="rows"/> are not of the same length.</exception>
    public async Task WriteAsync(IReadOnlyCollection<IReadOnlyCollection<T>> rows)
    {
        foreach (IReadOnlyCollection<T> row in rows)
            await WriteSingleRowAsync(row).ConfigureAwait(false);
    }

    private async Task WriteSingleRowAsync<X>(IReadOnlyCollection<X> row) where X : IConvertible
    {
        if (_firstRowSize < 0)
            _firstRowSize = row.Count;

        if (_firstRowSize != row.Count)
            CommonExceptions.ThrowInvalidOperation(Resources.RowsMustHaveSameSize);

        _stringBuilder.Clear(); // re-using memory, capacity stays untouched

        bool isFirst = true;
        foreach (X item in row)
        {
            if (!isFirst)
                _stringBuilder.Append(_separator);

            _stringBuilder.Append(item.ToString(CultureInfo.InvariantCulture));
            isFirst = false;
        }

        await _stream.WriteLineAsync(_stringBuilder).ConfigureAwait(false);
    }
}
