using System.Collections.Generic;
using System.IO;

using static System.FormattableString;

namespace BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat.Internal
{
    internal static class CoordinatesSectionSaver
    {
        public static void Save(StreamWriter writer, IHeaderFileContentVer1 content)
        {
            List<Coordinates>? channelCoordinates = content.GetChannelCoordinates();
            if (channelCoordinates != null)
            {
                writer.WriteLine();
                writer.WriteLine(IniFormat.FormatSectionName(Definitions.GetSectionName(Definitions.Section.Coordinates)!));
                FileSaverCommon.WriteCommentBlock(writer, content.InlinedComments.BelowCoordinatesInfosSection);

                for (int ch = 0; ch < channelCoordinates.Count; ++ch)
                {
                    Coordinates coordinates = channelCoordinates[ch];

                    string keyName = Invariant($"{Definitions.KeyChPlaceholder}{ch + 1}");
                    string keyValue = Invariant($"{coordinates.Radius},{coordinates.Theta},{coordinates.Phi}");

                    string line = IniFormat.FormatKeyValueLine(keyName, keyValue);
                    writer.WriteLine(line);
                }
            }
        }
    }
}
