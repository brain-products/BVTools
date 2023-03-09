using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat.Properties;
using static System.FormattableString;

namespace BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat;

[Serializable]
public class InvalidMarkerFileFormatException : Exception
{
    [ExcludeFromCodeCoverage]
    public InvalidMarkerFileFormatException() { }

    public InvalidMarkerFileFormatException(int lineNumber, string info)
        : base(Invariant($"{Resources.StandardExceptionText} {Resources.Line} {lineNumber + 1}: {info}.")) { }// +1 because for user indexing should be 1-based rather than 0-based

    public InvalidMarkerFileFormatException(string info)
        : base(Invariant($"{Resources.StandardExceptionText} {info}")) { }

    [ExcludeFromCodeCoverage]
    public InvalidMarkerFileFormatException(string message, Exception innerException) : base(message, innerException) { }

    [ExcludeFromCodeCoverage]
    protected InvalidMarkerFileFormatException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
