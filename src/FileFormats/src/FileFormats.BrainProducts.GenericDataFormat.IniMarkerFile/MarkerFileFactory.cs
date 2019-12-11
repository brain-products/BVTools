using System.IO;
using BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat.Internal;

namespace BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat
{
    /// <summary>
    /// Methods OpenForRead(), OpenForReadWrite(), CreateEmpty() simplify calls to Open() method for standard use-cases
    /// </summary>
    public static class MarkerFileFactory
    {
        /// <exception cref="InvalidMarkerFileFormatException">Thrown if file content does not comply with the BrainVision Data format.</exception>
        public static IMarkerFile Open(string path, FileMode mode, FileAccess access, FileShare share) =>
            new MarkerFile(path, mode, access, share);

        /// <exception cref="InvalidMarkerFileFormatException">Thrown if file content does not comply with the BrainVision Data format.</exception>
        public static IMarkerFile OpenForRead(string path) =>
            Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);

        /// <exception cref="InvalidMarkerFileFormatException">Thrown if file content does not comply with the BrainVision Data format.</exception>
        public static IMarkerFile OpenForReadWrite(string path) =>
            Open(path, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);

        public static IMarkerFile CreateEmpty(string path) =>
            Open(path, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.Read);
    }
}
