namespace BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat.Internal;

internal static class Definitions
{
    public const string IdentificationText = "BrainVision Data Exchange Header File Version";
    public const string IdentificationTextOldFashion = "Brain Vision Data Exchange Header File Version";

    // well-known sections
    private static readonly string?[] s_sectionNames = new string?[] { null, "Common Infos", "Binary Infos", "Channel Infos", "Coordinates", "Comment" };
    public enum Section { Unknown = -1, NoSection = 0, CommonInfos = 1, BinaryInfos = 2, ChannelInfos = 3, Coordinates = 4, Comment = 5 };
    public static string? GetSectionName(Section section) => s_sectionNames[(int)section];
    public static Section ParseSectionName(string sectionName) =>
        (Section)Array.FindIndex(s_sectionNames, p => sectionName.Equals(p, StringComparison.OrdinalIgnoreCase));

    // well-known section keys

    /// <summary>
    /// the order of the enum defines the order of keys in saved file
    /// </summary>
    public enum CommonInfosKeys
    {
        Codepage, DataFile, MarkerFile, DataFormat, DataOrientation, DataType, NumberOfChannels, SamplingInterval,
        SegmentationType, SegmentDataPoints, Averaged, AveragedSegments,
    }

    /// <summary>
    /// the order of the enum defines the order of keys in saved file
    /// </summary>
    public enum BinaryInfosKeys
    {
        BinaryFormat,
    }

    public const string Utf8Enum = "UTF-8";
    public const string KeyChPlaceholder = "Ch";
    public const char PlaceholderForCommaChar = '\u0001';

    //Although BrainVision claims it is UTF-8 compliant, it uses ANSI character to describe µ rather than Unicode one
    public const string Micro = "\u00B5"; // standard ANSI character 'µ'
    public const string MicroVolt = Micro + "V";
}
