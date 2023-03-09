using BrainVision.Lab.FileFormats.PublicDomain.BidsFormat;

namespace BrainVision.Lab.FileFormats.Internal;

internal static class Common
{
    public const string Micro = "\u00B5"; // BVCF uses standard ANSI character 'µ' rather than Unicode one
    public const string MicroVolt = Micro + "V";
    public const string MicroSiemens = Micro + "S";

    public static double GetSamplingFrequencyFromSamplingInterval(double samplingIntervalInMicroSec)
         => 1000000.0 / samplingIntervalInMicroSec;

    public static PrefixedUnit? ConvertTextToPrefixedUnit(string? txt) => txt switch
    {
        // Volt
        "V" => new PrefixedUnit(Unit.V),
        "mV" => new PrefixedUnit(Multiple.Milli, Unit.V),
        MicroVolt => new PrefixedUnit(Multiple.Micro, Unit.V),
        "nV" => new PrefixedUnit(Multiple.Nano, Unit.V),

        // Siemens
        "S" => new PrefixedUnit(Unit.Siemens),
        "mS" => new PrefixedUnit(Multiple.Milli, Unit.Siemens),
        MicroSiemens => new PrefixedUnit(Multiple.Micro, Unit.Siemens),
        "nS" => new PrefixedUnit(Multiple.Nano, Unit.Siemens),

        // other
        "C" => new PrefixedUnit(Unit.Celsius),

        //"mg" acceleration: m/s^2 in g=9.81
        //    => PrefixedUnit.None;
        //"ARU" // Arbitrary respiration unit
        //    => PrefixedUnit.None;

        _ => null,
    };
}
