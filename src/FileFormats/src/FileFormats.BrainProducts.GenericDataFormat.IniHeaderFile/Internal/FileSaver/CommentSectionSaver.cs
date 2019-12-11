using System.IO;

namespace BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat.Internal
{
    internal class CommentSectionSaver
    {
        public static void Save(StreamWriter writer, IHeaderFileContentVer1 content)
        {
            if (content.Comment != null)
            {
                writer.WriteLine();
                writer.WriteLine(IniFormat.FormatSectionName(Definitions.GetSectionName(Definitions.Section.Comment)!));
                FileSaverCommon.WriteCommentBlock(writer, content.InlinedComments.BelowCommentSection);
                writer.WriteLine(content.Comment);
            }
        }
    }
}
