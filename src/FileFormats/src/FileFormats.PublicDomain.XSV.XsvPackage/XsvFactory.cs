using System;
using BrainVision.Lab.FileFormats.PublicDomain.XSV.Internal;

namespace BrainVision.Lab.FileFormats.PublicDomain.XSV
{
    public static class XsvFactory
    {
        /// <exception cref="NotSupportedException">Thrown if <typeparamref name="T"/> is not one of supported types: int, double, string.</exception>
        public static IXsvWriter<T> CreateWriter<T>(string fileName, char separator) where T : IConvertible =>
                new XsvWriter<T>(fileName, separator);

        /// <exception cref="NotSupportedException">Thrown if <typeparamref name="T"/> is not one of supported types: int, double, string.</exception>
        public static IXsvReader<T> CreateReader<T>(string fileName, char separator, bool hasHeader) where T : IConvertible =>
            new XsvReader<T>(fileName, separator, hasHeader);
    }
}
