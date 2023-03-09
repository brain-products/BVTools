using System.Globalization;
using System.Text;
using static System.FormattableString;

namespace BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat.Internal;

internal static class MarkerInfosSectionSaver
{
    public static async Task SaveAsync(StreamWriter writer, IMarkerFileContentVer1 content)
    {
        IList<MarkerInfo>? markers = content.GetMarkers();
        if (markers != null)
        {
            await writer.WriteLineAsync().ConfigureAwait(false);
            await writer.WriteLineAsync(IniFormat.FormatSectionName(Definitions.GetSectionName(Definitions.Section.MarkerInfos)!)).ConfigureAwait(false);
            await FileSaverCommon.WriteCommentBlockAsync(writer, content.InlinedComments.BelowMarkerInfosSection).ConfigureAwait(false);

            for (int mk = 0; mk < markers.Count; ++mk)
            {
                MarkerInfo marker = markers[mk];

                string keyName = Invariant($"{Definitions.KeyMkPlaceholder}{mk + 1}");
                string keyValue = ConvertMarkerInfoToString(marker);

                string line = IniFormat.FormatKeyValueLine(keyName, keyValue);
                await writer.WriteLineAsync(line).ConfigureAwait(false);
            }
        }
    }

    private static string ConvertMarkerInfoToString(MarkerInfo markerInfo)
    {
        StringBuilder sb = new();

        string markerInfoType = markerInfo.Type.Replace(',', Definitions.PlaceholderForCommaChar);
        string markerInfoDescription = markerInfo.Description.Replace(',', Definitions.PlaceholderForCommaChar);

        // obligatory
        // Position and ChannelNumber are stored in file as 1-indexed
        sb.Append(Invariant($"{markerInfoType},{markerInfoDescription},{markerInfo.Position + 1},{markerInfo.Length},{markerInfo.ChannelNumber + 1}"));

        // optional
        if (markerInfo.Date != null)
            sb.Append(',').Append(markerInfo.Date.Value.ToString("yyyyMMddHHmmssffffff", CultureInfo.InvariantCulture));

        return sb.ToString();
    }
}
