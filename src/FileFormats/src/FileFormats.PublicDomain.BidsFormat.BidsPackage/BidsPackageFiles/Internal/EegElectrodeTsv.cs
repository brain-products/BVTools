using System.Diagnostics;
using System.Globalization;

namespace BrainVision.Lab.FileFormats.PublicDomain.BidsFormat.Internal;

internal sealed class EegElectrodeTsv : IConvertibleToStringTable
{
    private readonly EegElectrode _m;
    public EegElectrodeTsv(EegElectrode eegElectrode)
        => _m = eegElectrode;

    private static readonly string[] s_tsvIdentifiers = new string[]
    {
        "name",
        "x",
        "y",
        "z",
        "type",
        "material",
        "impedance",
    };

    public static IReadOnlyList<string> TsvIdentifiers => s_tsvIdentifiers;

    public List<string?> ToList()
    {
        List<string?> items = new()
        {
            //REQUIRED
            _m.Name,
            _m.X.ToString(CultureInfo.InvariantCulture),
            _m.Y.ToString(CultureInfo.InvariantCulture),
            _m.Z.ToString(CultureInfo.InvariantCulture),
            //RECOMMENDED
            _m.Type,
            _m.Material,
            _m.Impedance?.ToString(CultureInfo.InvariantCulture),
        };

        Debug.Assert(items.Count == s_tsvIdentifiers.Length);

        return items;
    }
}
