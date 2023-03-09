namespace BrainVision.Lab.FileFormats.PublicDomain.BidsFormat.Internal;

internal static class DirectoryExt
{
    public static void CreateDirectory(string? filepath)
    {
        if (!string.IsNullOrEmpty(filepath))
            Directory.CreateDirectory(filepath!);
    }
}
