using System.Globalization;

namespace BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat.Internal;

internal static class FileLoaderCommon
{
    public static bool TryParseChannelNumber(string keyName, out int channelNumber)
    {
        if (keyName.StartsWith(Definitions.KeyChPlaceholder, true, CultureInfo.InvariantCulture))
        {
            string textExpectedToContainChannelNumber = keyName[Definitions.KeyChPlaceholder.Length..];
            if (int.TryParse(textExpectedToContainChannelNumber, out channelNumber))
                return true;
        }

        channelNumber = -1;
        return false;
    }

    public static string ConcatenateWithNewLine(string? lineA, string lineB) =>
        (lineA == null) ? lineB : $"{lineA}{Environment.NewLine}{lineB}";
}
