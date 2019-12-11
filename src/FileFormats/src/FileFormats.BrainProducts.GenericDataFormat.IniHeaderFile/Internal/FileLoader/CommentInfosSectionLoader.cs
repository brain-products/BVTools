namespace BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat.Internal
{
    internal static class CommentInfosSectionLoader
    {
        public static void TryProcess(HeaderFileContent content, string line) =>
            content.Comment = FileLoaderCommon.ConcatenateWithNewLine(content.Comment, line);
    }
}
