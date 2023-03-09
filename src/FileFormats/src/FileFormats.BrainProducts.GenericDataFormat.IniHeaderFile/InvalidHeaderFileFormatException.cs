using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat.Properties;
using static System.FormattableString;

namespace BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat;

[Serializable]
public class InvalidHeaderFileFormatException : Exception
{
    [ExcludeFromCodeCoverage]
    public InvalidHeaderFileFormatException() { }

    public InvalidHeaderFileFormatException(int lineNumber, string info)
        : base(Invariant($"{Resources.StandardExceptionText} {Resources.Line} {lineNumber + 1}: {info}.")) { }// +1 because for user indexing should be 1-based rather than 0-based

    public InvalidHeaderFileFormatException(string info)
        : base(Invariant($"{Resources.StandardExceptionText} {info}")) { }

    [ExcludeFromCodeCoverage]
    public InvalidHeaderFileFormatException(string message, Exception innerException) : base(message, innerException) { }

    [ExcludeFromCodeCoverage]
    protected InvalidHeaderFileFormatException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
