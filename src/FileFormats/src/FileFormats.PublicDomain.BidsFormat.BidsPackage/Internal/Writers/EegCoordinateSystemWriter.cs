using BrainVision.Lab.FileFormats.PublicDomain.BidsFormat.Internal.IO;

namespace BrainVision.Lab.FileFormats.PublicDomain.BidsFormat.Internal.Writers
{
    internal static class EegCoordinateSystemWriter
    {
        public static void Save(string filePath, EegCoordinateSystem eegCoordinateSystem)
            => JsonWriter.Save(filePath, eegCoordinateSystem);
    }
}
