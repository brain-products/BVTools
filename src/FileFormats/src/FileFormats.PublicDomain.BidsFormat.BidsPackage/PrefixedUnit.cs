using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using BrainVision.Lab.FileFormats.PublicDomain.BidsFormat.Internal;

namespace BrainVision.Lab.FileFormats.PublicDomain.BidsFormat;

[DebuggerDisplay("{ToString()}")]
public readonly struct PrefixedUnit : IEquatable<PrefixedUnit>
{
    public static PrefixedUnit None { get; } = new PrefixedUnit(Multiple.None, Unit.None);

    public PrefixedUnit(Unit u)
    {
        M = Multiple.None;
        U = u;
    }

    public PrefixedUnit(Multiple m, Unit u)
    {
        M = m;
        U = u;
    }

    public Multiple M { get; }
    public Unit U { get; }

    public override string ToString()
        => U == Unit.None ? string.Empty : $"{(M == Multiple.None ? string.Empty : M.ToCustomString())}{U.ToCustomString()}";

    [ExcludeFromCodeCoverage]
    public override bool Equals(object? obj)
        => obj is PrefixedUnit unit && Equals(unit);

    [ExcludeFromCodeCoverage]
    public override int GetHashCode()
        => HashCode.Combine(M, U);

    [ExcludeFromCodeCoverage]
    public static bool operator ==(PrefixedUnit left, PrefixedUnit right)
        => left.Equals(right);

    [ExcludeFromCodeCoverage]
    public static bool operator !=(PrefixedUnit left, PrefixedUnit right)
        => !(left == right);

    [ExcludeFromCodeCoverage]
    public bool Equals(PrefixedUnit other)
        => M == other.M && U == other.U;
}
