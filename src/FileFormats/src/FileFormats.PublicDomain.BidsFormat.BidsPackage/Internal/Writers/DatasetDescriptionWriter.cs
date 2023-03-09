using BrainVision.Lab.FileFormats.PublicDomain.BidsFormat.Internal.IO;

namespace BrainVision.Lab.FileFormats.PublicDomain.BidsFormat.Internal.Writers;

internal static class DatasetDescriptionWriter
{
    public static async Task SaveAsync(string filePath, DatasetDescription datasetDescription)
        => await JsonWriter.SaveAsync(filePath, datasetDescription).ConfigureAwait(false);
}
