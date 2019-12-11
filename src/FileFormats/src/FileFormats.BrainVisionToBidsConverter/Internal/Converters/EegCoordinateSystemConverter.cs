using BrainVision.Lab.FileFormats.PublicDomain.BidsFormat;

namespace BrainVision.Lab.FileFormats.Internal.Converters
{
    internal static class EegCoordinateSystemConverter
    {
        public static EegCoordinateSystem Collect()
        {
            EegCoordinateSystem eegCoordinateSystem = new EegCoordinateSystem(
                //REQUIRED
                eegCoordinateSystem: CoordinateSystem.BESA,
                eegCoordinateUnits: new PrefixedUnit(Multiple.m, Unit.m))
            {
                #region Electrode positions fields
                //RECOMMENDED
                //EEGCoordinateSystemDescription = null,
                #endregion
            };

            return eegCoordinateSystem;
        }
    }
}
