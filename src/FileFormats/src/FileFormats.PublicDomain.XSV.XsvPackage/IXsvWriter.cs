using System;
using System.Collections.Generic;

namespace BrainVision.Lab.FileFormats.PublicDomain.XSV
{
    public interface IXsvWriter<T> : IDisposable where T : IConvertible
    {
        void WriteHeader(IReadOnlyCollection<string> headerRow);

        /// <exception cref="InvalidOperationException">Thrown if at least one of the <paramref name="rows"/> contains incorrect number of items.</exception>
        void Write(IReadOnlyCollection<IReadOnlyCollection<T>> rows);
    }
}
