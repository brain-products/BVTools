using BrainVision.Lab.FileFormats.PublicDomain.BidsFormat.Internal.IO;

namespace BrainVision.Lab.FileFormats.PublicDomain.BidsFormat.Internal.Writers;

internal static class EegCoordinateSystemWriter
{
    public static async Task SaveAsync(string filePath, EegCoordinateSystem eegCoordinateSystem)
        => await JsonWriter.SaveAsync(filePath, eegCoordinateSystem).ConfigureAwait(false);
}
