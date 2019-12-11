using System;
using System.IO;

namespace BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat.Internal
{
    internal static class FileSaverCommon
    {
        public static void WriteCommentBlock(StreamWriter writer, string? textToBeCommented)
        {
            if (textToBeCommented != null)
            {
                string[] separators = new string[] { Environment.NewLine };
                string[] lines = textToBeCommented.Split(separators, StringSplitOptions.None);

                foreach (string line in lines)
                {
                    writer.Write(';');
                    writer.WriteLine(line);
                }
            }
        }
    }
}
