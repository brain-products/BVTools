using BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat.Internal;

namespace BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat;

/// <summary>
/// Methods OpenForRead(), OpenForReadWrite(), CreateEmpty() simplify calls to Open() method for standard use-cases
/// </summary>
public static class HeaderFileFactory
{
    /// <exception cref="InvalidHeaderFileFormatException">Thrown if file content does not comply with the BrainVision Data format.</exception>
    public static IHeaderFile Open(string path, FileMode mode, FileAccess access, FileShare share) =>
        new HeaderFile(path, mode, access, share);

    /// <exception cref="InvalidHeaderFileFormatException">Thrown if file content does not comply with the BrainVision Data format.</exception>
    public static IHeaderFile OpenForRead(string path) =>
        Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);

    /// <exception cref="InvalidHeaderFileFormatException">Thrown if file content does not comply with the BrainVision Data format.</exception>
    public static IHeaderFile OpenForReadWrite(string path) =>
        Open(path, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);

    public static IHeaderFile CreateEmpty(string path) =>
        Open(path, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.Read);
}
