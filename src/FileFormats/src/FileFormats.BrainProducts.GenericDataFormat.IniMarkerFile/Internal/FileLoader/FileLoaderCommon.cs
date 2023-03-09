namespace BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat.Internal;

internal static class FileLoaderCommon
{
    public static string ConcatenateWithNewLine(string? lineA, string lineB) =>
        (lineA == null) ? lineB : $"{lineA}{Environment.NewLine}{lineB}";
}
