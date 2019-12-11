using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

using static System.FormattableString;

namespace BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat.Internal
{
    internal static class MarkerInfosSectionSaver
    {
        public static void Save(StreamWriter writer, IMarkerFileContentVer1 content)
        {
            List<MarkerInfo>? markers = content.GetMarkers();
            if (markers != null)
            {
                writer.WriteLine();
                writer.WriteLine(IniFormat.FormatSectionName(Definitions.GetSectionName(Definitions.Section.MarkerInfos)!));
                FileSaverCommon.WriteCommentBlock(writer, content.InlinedComments.BelowMarkerInfosSection);

                for (int mk = 0; mk < markers.Count; ++mk)
                {
                    MarkerInfo marker = markers[mk];

                    string keyName = Invariant($"{Definitions.KeyMkPlaceholder}{mk + 1}");
                    string keyValue = ConvertMarkerInfoToString(marker);

                    string line = IniFormat.FormatKeyValueLine(keyName, keyValue);
                    writer.WriteLine(line);
                }
            }
        }

        private static string ConvertMarkerInfoToString(MarkerInfo markerInfo)
        {
            StringBuilder sb = new StringBuilder();

            string markerInfoType = markerInfo.Type.Replace(',', Definitions.PlaceholderForCommaChar);
            string markerInfoDescription = markerInfo.Description.Replace(',', Definitions.PlaceholderForCommaChar);

            // obligatory
            // Position and ChannelNumber are stored in file as 1-indexed
            sb.Append(Invariant($"{markerInfoType},{markerInfoDescription},{markerInfo.Position + 1},{markerInfo.Length},{markerInfo.ChannelNumber + 1}"));

            // optional
            if (markerInfo.Date != null)
                sb.Append(",").Append(markerInfo.Date.Value.ToString("yyyyMMddHHmmssffffff", CultureInfo.InvariantCulture));

            return sb.ToString();
        }
    }
}
