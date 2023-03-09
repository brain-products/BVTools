using BrainVision.Lab.FileFormats.PublicDomain.BidsFormat.Internal.IO;

namespace BrainVision.Lab.FileFormats.PublicDomain.BidsFormat.Internal.Writers;

internal static class ReadmeWriter
{
    public static async Task SaveAsync(string filePath, string fileContent)
        => await PlainTextWriter.SaveAsync(filePath, fileContent).ConfigureAwait(false);
}
