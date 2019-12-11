using BrainVision.Lab.FileFormats.PublicDomain.BidsFormat.Internal.IO;

namespace BrainVision.Lab.FileFormats.PublicDomain.BidsFormat.Internal.Writers
{
    internal static class EegSidecarWriter
    {
        public static void Save(string filePath, EegSidecar sidecar)
            => JsonWriter.Save(filePath, sidecar);
    }
}
