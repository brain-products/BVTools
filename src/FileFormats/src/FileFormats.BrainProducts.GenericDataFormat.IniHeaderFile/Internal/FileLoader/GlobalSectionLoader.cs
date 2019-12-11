using System.Diagnostics.CodeAnalysis;
using BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat.Properties;

namespace BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat.Internal
{
    internal static class GlobalSectionLoader
    {
        // no keys allowed in section-less area of the file
        public static bool TryProcess(string keyName, [NotNullWhen(false)] out string? exceptionMessage) // (HeaderFileContent content, string keyName, string keyValue)
        {
            exceptionMessage = $"{Resources.UnrecognizedKey} {keyName}";
            return false;
        }
    }
}
