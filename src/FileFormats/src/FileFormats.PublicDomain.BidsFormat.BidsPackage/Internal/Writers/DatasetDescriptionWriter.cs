using BrainVision.Lab.FileFormats.PublicDomain.BidsFormat.Internal.IO;

namespace BrainVision.Lab.FileFormats.PublicDomain.BidsFormat.Internal.Writers
{
    internal class DatasetDescriptionWriter
    {
        public static void Save(string filePath, DatasetDescription datasetDescription)
            => JsonWriter.Save(filePath, datasetDescription);
    }
}
