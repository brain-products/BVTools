using System;
using System.Collections.Generic;

namespace BrainVision.Lab.FileFormats.PublicDomain.XSV
{
    public interface IXsvReader<T> : IDisposable where T : IConvertible
    {
        IReadOnlyList<string>? HeaderRow { get; }

        /// <exception cref="InvalidXsvFileFormatException">Thrown if at least one row contains incorrect number of items or if an item cannot not be converted to T (has invalid format).</exception>
        List<T[]> ReadToEnd();

        /// <exception cref="InvalidXsvFileFormatException">Thrown if at least one row contains incorrect number of items or if an item cannot not be converted to T (has invalid format).</exception>
        IEnumerable<T[]> EnumerateRows();
    }
}
