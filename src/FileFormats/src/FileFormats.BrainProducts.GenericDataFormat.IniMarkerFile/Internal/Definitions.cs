namespace BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat.Internal;

internal static class Definitions
{
    public const string IdentificationText = "BrainVision Data Exchange Marker File Version";
    public const string IdentificationTextOldFashion1 = "Brain Vision Data Exchange Marker File Version";
    public const string IdentificationTextOldFashion2 = "Brain Vision Data Exchange Marker File, Version";

    // well-known sections
    private static readonly string?[] s_sectionNames = new string?[] { null, "Common Infos", "Marker Infos" };
    public enum Section { Unknown = -1, NoSection = 0, CommonInfos = 1, MarkerInfos = 2 };
    public static string? GetSectionName(Section section) => s_sectionNames[(int)section];
    public static Section ParseSectionName(string sectionName) =>
        (Section)Array.FindIndex(s_sectionNames, p => sectionName.Equals(p, StringComparison.OrdinalIgnoreCase));

    // well-known section keys

    /// <summary>
    /// the order of the enum defines the order of keys in saved file
    /// </summary>
    public enum CommonInfosKeys
    {
        Codepage, DataFile
    }

    public const string Utf8Enum = "UTF-8";
    public const string KeyMkPlaceholder = "Mk";
    public const char PlaceholderForCommaChar = '\u0001';
}
