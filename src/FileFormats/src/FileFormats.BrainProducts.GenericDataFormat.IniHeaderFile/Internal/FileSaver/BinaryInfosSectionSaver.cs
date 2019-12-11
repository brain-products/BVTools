using System;
using System.IO;

namespace BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat.Internal
{
    internal static class BinaryInfosSectionSaver
    {
        public static void Save(StreamWriter writer, IHeaderFileContentVer1 content)
        {
            writer.WriteLine();
            writer.WriteLine(IniFormat.FormatSectionName(Definitions.GetSectionName(Definitions.Section.BinaryInfos)!));
            FileSaverCommon.WriteCommentBlock(writer, content.InlinedComments.BelowBinaryInfosSection);

            foreach (Definitions.BinaryInfosKeys key in (Definitions.BinaryInfosKeys[])Enum.GetValues(typeof(Definitions.BinaryInfosKeys)))
            {
                string? keyValue = key switch
                {
                    Definitions.BinaryInfosKeys.BinaryFormat => content.BinaryFormat?.ToString(),
                    _ => throw new NotImplementedException(), // should never happen
                };

                if (keyValue != null)
                {
                    string line = IniFormat.FormatKeyValueLine(key.ToString(), keyValue);
                    writer.WriteLine(line);
                }
            }
        }
    }
}
