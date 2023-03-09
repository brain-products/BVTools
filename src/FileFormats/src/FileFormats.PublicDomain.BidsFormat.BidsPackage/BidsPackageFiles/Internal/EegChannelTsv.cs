using System.Diagnostics;
using System.Globalization;

namespace BrainVision.Lab.FileFormats.PublicDomain.BidsFormat.Internal;

internal sealed class EegChannelTsv : IConvertibleToStringTable
{
    private readonly EegChannel _m;
    public EegChannelTsv(EegChannel eegChannel)
        => _m = eegChannel;

    private static readonly string[] s_tsvIdentifiers = new string[]
    {
        "name",
        "type",
        "units",
        "description",
        "sampling_frequency",
        "reference",
        "low_cutoff",
        "high_cutoff",
        "notch",
        "status",
        "status_description",
    };

    public static IReadOnlyList<string> TsvIdentifiers => s_tsvIdentifiers;

    public List<string?> ToList()
    {
        List<string?> items = new()
        {
            //REQUIRED
            _m.Name,
            _m.Type.ToString(),
            _m.Units.ToString(),
            //OPTIONAL
            _m.Description,
            _m.SamplingFrequency?.ToString(CultureInfo.InvariantCulture),
            _m.Reference,
            _m.LowCutoff,
            _m.HighCutoff,
            _m.Notch,
            _m.Status,
            _m.StatusDescription,
        };

        Debug.Assert(items.Count == s_tsvIdentifiers.Length);

        return items;
    }
}
