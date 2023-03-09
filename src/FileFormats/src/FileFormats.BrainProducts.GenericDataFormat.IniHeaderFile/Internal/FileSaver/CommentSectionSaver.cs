namespace BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat.Internal;

internal static class CommentSectionSaver
{
    public static async Task SaveAsync(StreamWriter writer, IHeaderFileContentVer1 content)
    {
        if (content.Comment != null)
        {
            await writer.WriteLineAsync().ConfigureAwait(false);
            await writer.WriteLineAsync(IniFormat.FormatSectionName(Definitions.GetSectionName(Definitions.Section.Comment)!)).ConfigureAwait(false);
            await FileSaverCommon.WriteCommentBlockAsync(writer, content.InlinedComments.BelowCommentSection).ConfigureAwait(false);
            await writer.WriteLineAsync(content.Comment).ConfigureAwait(false);
        }
    }
}
