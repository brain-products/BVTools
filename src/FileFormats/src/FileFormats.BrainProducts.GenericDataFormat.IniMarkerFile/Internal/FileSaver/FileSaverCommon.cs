namespace BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat.Internal;

internal static class FileSaverCommon
{
    public static async Task WriteCommentBlockAsync(StreamWriter writer, string? textToBeCommented)
    {
        if (textToBeCommented != null)
        {
            string[] separators = new string[] { Environment.NewLine };
            string[] lines = textToBeCommented.Split(separators, StringSplitOptions.None);

            foreach (string line in lines)
            {
                await writer.WriteAsync(';').ConfigureAwait(false);
                await writer.WriteLineAsync(line).ConfigureAwait(false);
            }
        }
    }
}
