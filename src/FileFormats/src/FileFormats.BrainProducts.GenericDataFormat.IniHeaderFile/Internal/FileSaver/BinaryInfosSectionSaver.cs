namespace BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat.Internal;

internal static class BinaryInfosSectionSaver
{
    public static async Task SaveAsync(StreamWriter writer, IHeaderFileContentVer1 content)
    {
        await writer.WriteLineAsync().ConfigureAwait(false);
        await writer.WriteLineAsync(IniFormat.FormatSectionName(Definitions.GetSectionName(Definitions.Section.BinaryInfos)!)).ConfigureAwait(false);
        await FileSaverCommon.WriteCommentBlockAsync(writer, content.InlinedComments.BelowBinaryInfosSection).ConfigureAwait(false);

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
                await writer.WriteLineAsync(line).ConfigureAwait(false);
            }
        }
    }
}
