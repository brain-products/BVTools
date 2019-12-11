using System.IO;
using System.Text;

namespace BrainVision.Lab.FileFormats.PublicDomain.BidsFormat.Internal.IO
{
    internal static class PlainTextWriter
    {
        public static void Save(string filePath, string fileContent)
        {
            using TextWriter textWriter = new StreamWriter(filePath, false, Encoding.UTF8);
            textWriter.Write(fileContent);
        }
    }
}
