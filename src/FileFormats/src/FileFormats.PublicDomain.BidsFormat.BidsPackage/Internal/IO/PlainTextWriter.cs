using System.Text;

namespace BrainVision.Lab.FileFormats.PublicDomain.BidsFormat.Internal.IO;

internal static class PlainTextWriter
{
    public static async Task SaveAsync(string filePath, string fileContent)
    {
#pragma warning disable CA2000 // Dispose objects before losing scope
        TextWriter textWriter = new StreamWriter(filePath, false, Encoding.UTF8);
#pragma warning restore CA2000 // Dispose objects before losing scope
        await using (textWriter.ConfigureAwait(false))
        {
            await textWriter.WriteAsync(fileContent).ConfigureAwait(false);
        }
    }
}
