using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat
{
    [DebuggerDisplay("{Radius},{Theta},{Phi}")]
    public struct Coordinates : IEquatable<Coordinates>
    {
        /// <summary>
        /// Required
        /// </summary>
        public double Radius { get; set; }

        /// <summary>
        /// Required
        /// </summary>
        public double Theta { get; set; }

        /// <summary>
        /// Required
        /// </summary>
        public double Phi { get; set; }

        [ExcludeFromCodeCoverage]
        public override bool Equals(object obj)
        {
            if (!(obj is Coordinates))
                return false;

            return Equals((Coordinates)obj);
        }

        [ExcludeFromCodeCoverage]
        public override int GetHashCode() =>
            Radius.GetHashCode() ^ Theta.GetHashCode() ^ Phi.GetHashCode();

        [ExcludeFromCodeCoverage]
        public static bool operator ==(Coordinates left, Coordinates right) =>
            left.Equals(right);

        [ExcludeFromCodeCoverage]
        public static bool operator !=(Coordinates left, Coordinates right) =>
            !(left == right);

        [ExcludeFromCodeCoverage]
        public bool Equals(Coordinates other) =>
            Radius == other.Radius && Theta == other.Theta && Phi == other.Phi;
    }
}
