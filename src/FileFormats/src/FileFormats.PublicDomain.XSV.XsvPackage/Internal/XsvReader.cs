using System.Globalization;
using BrainVision.Lab.FileFormats.PublicDomain.XSV.Properties;
using BrainVision.Lab.SystemExt.Internal;

namespace BrainVision.Lab.FileFormats.PublicDomain.XSV.Internal;

internal sealed class XsvReader<T> : IXsvReader<T> where T : IConvertible
{
    private readonly StreamReader _stream;
    private readonly char _separator;
    private readonly bool _hasHeader;
    private readonly string[]? _header; // is null when no header is requested to read
    private readonly Type _tType = typeof(T);
    private readonly Type[] _supportedTypes = { typeof(int), typeof(double), typeof(string) };

    public XsvReader(string fileName, char separator, bool hasHeader)
    {
        if (!_supportedTypes.Any(type => type == _tType))
            CommonExceptions.ThrowNotSupported($"{Resources.UnsupportedType} ({_tType})");

        _stream = new StreamReader(fileName);
        _separator = separator;
        _hasHeader = hasHeader;

        if (_hasHeader)
        {
            string? headerLine = _stream.ReadLine();
            _header = headerLine != null ? headerLine.Split(_separator) : Array.Empty<string>();
        }
    }

    public void Dispose() => _stream.Dispose();

    /// <summary>
    /// returns null only when no header was requested to read
    /// </summary>
    public IReadOnlyList<string>? HeaderRow => _header;

    public async Task<IList<T[]>> ReadToEndAsync()
    {
        List<T[]> rows = new();

        await foreach (T[] row in EnumerateRowsAsync())
            rows.Add(row);

        return rows;
    }

    public async IAsyncEnumerable<T[]> EnumerateRowsAsync()
    {
        int lineNumber = -1;
        int expectedRowItemsLength = -1;

        if (_hasHeader)
        {
            expectedRowItemsLength = _header == null ? 0 : _header.Length;
            ++lineNumber;
        }

        string? line;
        while ((line = await _stream.ReadLineAsync().ConfigureAwait(false)) != null)
        {
            ++lineNumber;

            string[] stringRowItems = line.Split(_separator);

            if (expectedRowItemsLength < 0)
                expectedRowItemsLength = stringRowItems.Length;

            if (stringRowItems.Length != expectedRowItemsLength)
                throw new InvalidXsvFileFormatException(lineNumber,
                    string.Format(CultureInfo.InvariantCulture, Resources.RowHasIncorrectNumberOfItems, new object[] { stringRowItems.Length, expectedRowItemsLength }));

            if (!TryConvert(stringRowItems, out T[] rowItems))
                throw new InvalidXsvFileFormatException(lineNumber, Resources.RowHasIncorrectFormat);

            yield return rowItems;
        }
    }

    private bool TryConvert(string[] stringRowItems, out T[] rowItems)
    {
        rowItems = new T[stringRowItems.Length];

        for (int i = 0; i < stringRowItems.Length; ++i)
        {
            string item = stringRowItems[i];

            T tValue;
            if (_tType == typeof(int))
            {
                if (int.TryParse(item, NumberStyles.Integer, CultureInfo.InvariantCulture, out int val))
                    tValue = (T)(val as IConvertible); // Warning! this causes boxing
                else
                    return false;
            }
            else if (_tType == typeof(double))
            {
                if (double.TryParse(item, NumberStyles.Float, CultureInfo.InvariantCulture, out double val))
                    tValue = (T)(val as IConvertible); // Warning! this causes boxing
                else
                    return false;
            }
            else if (_tType == typeof(string))
            {
                tValue = (T)(item as IConvertible);
            }
            else
            {
                throw new NotImplementedException(); // should never happen
            }

            rowItems[i] = tValue;
        }

        return true;
    }
}
