using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

using static System.FormattableString;

namespace BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat.Internal
{
    internal static class ChannelInfosSectionSaver
    {
        public static void Save(StreamWriter writer, IHeaderFileContentVer1 content)
        {
            List<ChannelInfo>? channelInfos = content.GetChannelInfos();
            if (channelInfos != null)
            {
                writer.WriteLine();
                writer.WriteLine(IniFormat.FormatSectionName(Definitions.GetSectionName(Definitions.Section.ChannelInfos)!));
                FileSaverCommon.WriteCommentBlock(writer, content.InlinedComments.BelowChannelInfosSection);

                for (int ch = 0; ch < channelInfos.Count; ++ch)
                {
                    ChannelInfo channelInfo = channelInfos[ch];

                    string keyName = Invariant($"{Definitions.KeyChPlaceholder}{ch + 1}");
                    string keyValue = ConvertChannelInfoToString(channelInfo);

                    string line = IniFormat.FormatKeyValueLine(keyName, keyValue);
                    writer.WriteLine(line);
                }
            }
        }

        private static string ConvertChannelInfoToString(ChannelInfo channelInfo)
        {
            StringBuilder sb = new StringBuilder();

            // obligatory
            string channelInfoName = channelInfo.Name.Replace(',', Definitions.PlaceholderForCommaChar);
            string channelInfoRefName = channelInfo.RefName.Replace(',', Definitions.PlaceholderForCommaChar);
            string? channelInfoResolution = channelInfo.Resolution?.ToString(CultureInfo.InvariantCulture);

            sb.Append($"{channelInfoName},{channelInfoRefName},{channelInfoResolution}");

            // optional
            if (channelInfo.Unit != null)
                sb.Append($",{channelInfo.Unit}");

            return sb.ToString();
        }
    }
}
