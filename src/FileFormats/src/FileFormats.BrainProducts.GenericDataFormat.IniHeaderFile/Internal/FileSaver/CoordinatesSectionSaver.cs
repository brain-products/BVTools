using static System.FormattableString;

namespace BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat.Internal;

internal static class CoordinatesSectionSaver
{
    public static async Task SaveAsync(StreamWriter writer, IHeaderFileContentVer1 content)
    {
        IList<Coordinates>? channelCoordinates = content.GetChannelCoordinates();
        if (channelCoordinates != null)
        {
            await writer.WriteLineAsync().ConfigureAwait(false);
            await writer.WriteLineAsync(IniFormat.FormatSectionName(Definitions.GetSectionName(Definitions.Section.Coordinates)!)).ConfigureAwait(false);
            await FileSaverCommon.WriteCommentBlockAsync(writer, content.InlinedComments.BelowCoordinatesInfosSection).ConfigureAwait(false);

            for (int ch = 0; ch < channelCoordinates.Count; ++ch)
            {
                Coordinates coordinates = channelCoordinates[ch];

                string keyName = Invariant($"{Definitions.KeyChPlaceholder}{ch + 1}");
                string keyValue = Invariant($"{coordinates.Radius},{coordinates.Theta},{coordinates.Phi}");

                string line = IniFormat.FormatKeyValueLine(keyName, keyValue);
                await writer.WriteLineAsync(line).ConfigureAwait(false);
            }
        }
    }
}
