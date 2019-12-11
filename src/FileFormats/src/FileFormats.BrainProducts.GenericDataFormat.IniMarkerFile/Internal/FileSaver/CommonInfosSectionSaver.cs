using System;
using System.IO;

namespace BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat.Internal
{
    internal static class CommonInfosSectionSaver
    {
        public static void Save(StreamWriter writer, IMarkerFileContentVer1 content)
        {
            writer.WriteLine();
            writer.WriteLine(IniFormat.FormatSectionName(Definitions.GetSectionName(Definitions.Section.CommonInfos)!));
            FileSaverCommon.WriteCommentBlock(writer, content.InlinedComments.BelowCommonInfosSection);

            Definitions.CommonInfosKeys[] commonInfosKeys = (Definitions.CommonInfosKeys[])Enum.GetValues(typeof(Definitions.CommonInfosKeys));

            foreach (Definitions.CommonInfosKeys key in commonInfosKeys)
            {
                var keyValue = key switch
                {
                    Definitions.CommonInfosKeys.Codepage => (content.CodePage == Codepage.Utf8) ? Definitions.Utf8Enum : content.CodePage?.ToString(), // replacing Utf8 enum with Utf-8 string
                    Definitions.CommonInfosKeys.DataFile => content.DataFile,
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
