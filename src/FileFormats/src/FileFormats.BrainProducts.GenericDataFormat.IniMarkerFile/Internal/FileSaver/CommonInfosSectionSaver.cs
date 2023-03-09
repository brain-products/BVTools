using BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat.MarkerFileEnums;

namespace BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat.Internal;

internal static class CommonInfosSectionSaver
{
    public static async Task SaveAsync(StreamWriter writer, IMarkerFileContentVer1 content)
    {
        await writer.WriteLineAsync().ConfigureAwait(false);
        await writer.WriteLineAsync(IniFormat.FormatSectionName(Definitions.GetSectionName(Definitions.Section.CommonInfos)!)).ConfigureAwait(false);
        await FileSaverCommon.WriteCommentBlockAsync(writer, content.InlinedComments.BelowCommonInfosSection).ConfigureAwait(false);

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
                await writer.WriteLineAsync(line).ConfigureAwait(false);
            }
        }
    }
}
