using System.Runtime.Serialization;
using BrainVision.Lab.FileFormats.PublicDomain.XSV.Properties;

namespace BrainVision.Lab.FileFormats.PublicDomain.XSV;

[Serializable]
public class InvalidXsvFileFormatException : Exception
{
    public InvalidXsvFileFormatException() { }
    public InvalidXsvFileFormatException(string message) : base(message) { }
    public InvalidXsvFileFormatException(string message, Exception inner) : base(message, inner) { }
    public InvalidXsvFileFormatException(int lineNumber, string message) : this($"{Resources.LineNumber} {lineNumber + 1}: {message}") => LineNumber = lineNumber;
    protected InvalidXsvFileFormatException(
      SerializationInfo info,
      StreamingContext context) : base(info, context) { }

    /// <summary>
    /// 0-based line number in source file, where the error occurred
    /// </summary>
    public int LineNumber { get; }
}
