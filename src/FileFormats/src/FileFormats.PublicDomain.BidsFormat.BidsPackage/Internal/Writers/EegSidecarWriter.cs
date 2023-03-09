using BrainVision.Lab.FileFormats.PublicDomain.BidsFormat.Internal.IO;

namespace BrainVision.Lab.FileFormats.PublicDomain.BidsFormat.Internal.Writers;

internal static class EegSidecarWriter
{
    public static async Task SaveAsync(string filePath, EegSidecar sidecar)
        => await JsonWriter.SaveAsync(filePath, sidecar).ConfigureAwait(false);
}
