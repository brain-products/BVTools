using BrainVision.Lab.FileFormats.PublicDomain.BidsFormat;

namespace BrainVision.Lab.FileFormats.Internal
{
    internal static class Common
    {
        public const string Micro = "\u00B5"; // BVCF uses standard ANSI character 'µ' rather than Unicode one
        public const string MicroVolt = Micro + "V";
        public const string MicroSecond = Micro + "S";

        public static double GetSamplingFrequencyFromSamplingInterval(double samplingIntervalInMicroSec)
             => 1000000.0 / samplingIntervalInMicroSec;

        public static PrefixedUnit? ConvertTextToPrefixedUnit(string? txt)
        {
            PrefixedUnit? result = null;
            switch (txt)
            {
                // volts
                case "V":
                    result = new PrefixedUnit(Unit.V);
                    break;
                case "mV":
                    result = new PrefixedUnit(Multiple.m, Unit.V);
                    break;
                case MicroVolt:
                    result = new PrefixedUnit(Multiple.mi, Unit.V);
                    break;
                case "nV":
                    result = new PrefixedUnit(Multiple.n, Unit.V);
                    break;
                // seconds
                case "S":
                    result = new PrefixedUnit(Unit.S);
                    break;
                case "mS":
                    result = new PrefixedUnit(Multiple.m, Unit.S);
                    break;
                case MicroSecond:
                    result = new PrefixedUnit(Multiple.mi, Unit.S);
                    break;
                case "nS":
                    result = new PrefixedUnit(Multiple.n, Unit.S);
                    break;
                // other
                case "C":
                    result = new PrefixedUnit(Unit.Celsius);
                    break;
                    //case "mg": acceleration: m/s^2 in g=9.81
                    //    return PrefixedUnit.None;
                    //case "ARU": // Arbitrary respiration unit
                    //    return PrefixedUnit.None;
            }

            return result;
        }
    }
}
