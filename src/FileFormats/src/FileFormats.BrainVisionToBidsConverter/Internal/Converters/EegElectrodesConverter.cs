using BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat;
using BrainVision.Lab.FileFormats.PublicDomain.BidsFormat;

namespace BrainVision.Lab.FileFormats.Internal.Converters;

internal static class EegElectrodesConverter
{
    public static EegElectrodeCollection? Collect(BrainVisionPackage filesContent)
    {
        IHeaderFileContentVer1 headerContent = filesContent.HeaderFileContent;
        IList<Coordinates>? inputCoordinates = headerContent.GetChannelCoordinates();
        if (inputCoordinates == null)
            return null;

        IList<ChannelInfo>? inputChannels = headerContent.GetChannelInfos();
        int inputChannelsCount = inputChannels == null ? 0 : inputChannels.Count;

        EegElectrodeCollection eegElectrodes = new();

        for (int channelNumber = 0; channelNumber < inputCoordinates.Count; ++channelNumber)
        {
            Coordinates coordinates = inputCoordinates[channelNumber];

            (double cartesianX, double cartesianY, double cartesianZ) = SphericalToCartesian(coordinates);

            EegElectrode eegElectrode = new(
                //REQUIRED
                name: channelNumber < inputChannelsCount ? inputChannels![channelNumber].Name : string.Empty,
                x: cartesianX,
                y: cartesianY,
                z: cartesianZ)
            {
                //RECOMMENDED
                //Impedance = 0,
            };

            eegElectrodes.Add(eegElectrode);
        }

        return eegElectrodes;
    }

    /// <summary>
    /// copied from BrainVision.Lab.Mathlib.Coordinates
    /// </summary>
    private static (double X, double Y, double Z) SphericalToCartesian(Coordinates coords)
    {
        const double dConvFactor = Math.PI / 180.0;
        double thetaRad = dConvFactor * coords.Theta;
        double phiRad = dConvFactor * coords.Phi;
        double radius = coords.Radius;
        double x = radius * Math.Sin(thetaRad) * Math.Cos(phiRad);
        double y = radius * Math.Sin(thetaRad) * Math.Sin(phiRad);
        double z = radius * Math.Cos(thetaRad);
        return (x, y, z);
    }
}
