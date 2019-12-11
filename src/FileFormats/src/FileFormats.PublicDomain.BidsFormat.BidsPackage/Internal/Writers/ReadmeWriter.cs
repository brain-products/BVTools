using BrainVision.Lab.FileFormats.PublicDomain.BidsFormat.Internal.IO;

namespace BrainVision.Lab.FileFormats.PublicDomain.BidsFormat.Internal.Writers
{
    internal static class ReadmeWriter
    {
        public static void Save(string filePath, string fileContent)
            => PlainTextWriter.Save(filePath, fileContent);
    }
}
