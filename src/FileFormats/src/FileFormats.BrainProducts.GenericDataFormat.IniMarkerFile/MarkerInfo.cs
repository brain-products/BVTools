using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat;

/// <summary>
/// if specific key cannot be parsed or does not exist in a file, the value null or int.MinVal is returned
/// </summary>
[DebuggerDisplay("{Type},{Description},{Position},{Length},{ChannelNumber},{Date}")]
public struct MarkerInfo : IEquatable<MarkerInfo>
{
    /// <summary>
    /// Required
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// Required
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Required
    /// Position is 0-indexed, but it is stored in file as 1-indexed
    /// </summary>
    public int Position { get; set; }

    /// <summary>
    /// Required
    /// </summary>
    public int Length { get; set; }

    /// <summary>
    /// Required
    /// ChannelNumber is 0-indexed, but it is stored in file as 1-indexed
    /// -1 means attached to all channels (in file it is stored as 0)
    /// </summary>
    public int ChannelNumber { get; set; }

    /// <summary>
    /// Optional. If null, it will not be written to file.
    /// </summary>
    public DateTime? Date { get; set; }

    [ExcludeFromCodeCoverage]
    public override bool Equals(object? obj)
        => obj is MarkerInfo info && Equals(info);

    [ExcludeFromCodeCoverage]
    public override int GetHashCode()
    {
        int hashType = Type.GetHashCode(StringComparison.Ordinal);
        int hashDesc = Description.GetHashCode(StringComparison.Ordinal);
        int hashPos = Position;
        int hashLength = Length;
        int hashChannelNumber = ChannelNumber;
        int hashDate = (Date != null) ? Date.GetHashCode() : 0;

        return hashType ^ hashDesc ^ hashPos ^ hashLength ^ hashChannelNumber ^ hashDate;
    }

    [ExcludeFromCodeCoverage]
    public static bool operator ==(MarkerInfo left, MarkerInfo right) =>
        left.Equals(right);

    [ExcludeFromCodeCoverage]
    public static bool operator !=(MarkerInfo left, MarkerInfo right) =>
        !(left == right);

    [ExcludeFromCodeCoverage]
    public bool Equals(MarkerInfo other) =>
        Type == other.Type && Description == other.Description && Position == other.Position && Length == other.Length && ChannelNumber == other.ChannelNumber && Date == other.Date;
}
