using System.Globalization;
using System.Text;
using static System.FormattableString;

namespace BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat.Internal;

internal static class ChannelInfosSectionSaver
{
    public static async Task SaveAsync(StreamWriter writer, IHeaderFileContentVer1 content)
    {
        IList<ChannelInfo>? channelInfos = content.GetChannelInfos();
        if (channelInfos != null)
        {
            await writer.WriteLineAsync().ConfigureAwait(false);
            await writer.WriteLineAsync(IniFormat.FormatSectionName(Definitions.GetSectionName(Definitions.Section.ChannelInfos)!)).ConfigureAwait(false);
            await FileSaverCommon.WriteCommentBlockAsync(writer, content.InlinedComments.BelowChannelInfosSection).ConfigureAwait(false);

            for (int ch = 0; ch < channelInfos.Count; ++ch)
            {
                ChannelInfo channelInfo = channelInfos[ch];

                string keyName = Invariant($"{Definitions.KeyChPlaceholder}{ch + 1}");
                string keyValue = ConvertChannelInfoToString(channelInfo);

                string line = IniFormat.FormatKeyValueLine(keyName, keyValue);
                await writer.WriteLineAsync(line).ConfigureAwait(false);
            }
        }
    }

    private static string ConvertChannelInfoToString(ChannelInfo channelInfo)
    {
        StringBuilder sb = new();

        // obligatory
        string channelInfoName = channelInfo.Name.Replace(',', Definitions.PlaceholderForCommaChar);
        string channelInfoRefName = channelInfo.RefName.Replace(',', Definitions.PlaceholderForCommaChar);
        string? channelInfoResolution = channelInfo.Resolution?.ToString(CultureInfo.InvariantCulture);

        sb.Append(CultureInfo.InvariantCulture, $"{channelInfoName},{channelInfoRefName},{channelInfoResolution}");

        // optional
        if (channelInfo.Unit != null)
            sb.Append(CultureInfo.InvariantCulture, $",{channelInfo.Unit}");

        return sb.ToString();
    }
}
