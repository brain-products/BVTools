using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat
{
    /// <summary>
    /// if specific key cannot be parsed or does not exist in a file, the value null is returned
    /// </summary>
    [DebuggerDisplay("{Name},{RefName},{Resolution},{Unit}")]
    public struct ChannelInfo : IEquatable<ChannelInfo>
    {
        /// <summary>
        /// Required
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Required
        /// </summary>
        public string RefName { get; set; }

        /// <summary>
        /// Required but may be empty. Empty string is parsed to null, null is saved as empty string.
        /// </summary>
        public double? Resolution { get; set; }

        /// <summary>
        /// Optional. If null, it will not be written to file.
        /// </summary>
        public string? Unit { get; set; }

        [ExcludeFromCodeCoverage]
        public override bool Equals(object obj)
        {
            if (!(obj is ChannelInfo))
                return false;

            return Equals((ChannelInfo)obj);
        }

        [ExcludeFromCodeCoverage]
        public override int GetHashCode()
        {
            int hashName = Name.GetHashCode(StringComparison.Ordinal);
            int hashRef = RefName.GetHashCode(StringComparison.Ordinal);
            int hashRes = (Resolution != null) ? Resolution.GetHashCode() : 0;
            int hashUnit = (Unit != null) ? Unit.GetHashCode(StringComparison.Ordinal) : 0;

            return hashName ^ hashRef ^ hashRes ^ hashUnit;
        }

        [ExcludeFromCodeCoverage]
        public static bool operator ==(ChannelInfo left, ChannelInfo right) =>
            left.Equals(right);

        [ExcludeFromCodeCoverage]
        public static bool operator !=(ChannelInfo left, ChannelInfo right) =>
            !(left == right);

        [ExcludeFromCodeCoverage]
        public bool Equals(ChannelInfo other) =>
            Name == other.Name && RefName == other.RefName && Resolution == other.Resolution && Unit == other.Unit;
    }
}
