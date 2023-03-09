using System.Diagnostics.CodeAnalysis;
using BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat.HeaderFileEnums;
using BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat.Properties;

namespace BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat.Internal;

internal static class BinaryInfosSectionLoader
{
    public static bool TryProcess(HeaderFileContent content, string keyName, string keyValue, [NotNullWhen(false)] out string? exceptionMessage)
    {
        if (!Enum.TryParse(keyName, true, out Definitions.BinaryInfosKeys key))
        {
            exceptionMessage = $"{Resources.UnrecognizedKey} {keyName}";
            return false;
        }

        switch (key)
        {
            case Definitions.BinaryInfosKeys.BinaryFormat:
                {
                    if (!Enum.TryParse(keyValue, true, out BinaryFormat value))
                    {
                        exceptionMessage = $"{Resources.UnrecognizedKeyValue} {keyValue}";
                        return false;
                    }

                    content.BinaryFormat = value;
                }
                break;
            default:
                throw new NotImplementedException(); // should never happen
        }

        exceptionMessage = null;
        return true;
    }
}
