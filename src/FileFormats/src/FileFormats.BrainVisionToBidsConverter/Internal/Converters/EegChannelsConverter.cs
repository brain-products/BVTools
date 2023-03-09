using System.Globalization;
using BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat;
using BrainVision.Lab.FileFormats.Properties;
using BrainVision.Lab.FileFormats.PublicDomain.BidsFormat;

namespace BrainVision.Lab.FileFormats.Internal.Converters;

internal static class EegChannelsConverter
{
    public static EegChannelCollection? Collect(BrainVisionPackage filesContent)
    {
        IHeaderFileContentVer1 headerContent = filesContent.HeaderFileContent;
        IList<ChannelInfo>? inputChannels = headerContent.GetChannelInfos();
        if (inputChannels == null)
            return null;

        double samplingFrequency = Common.GetSamplingFrequencyFromSamplingInterval(headerContent.SamplingInterval!.Value);

        EegChannelCollection eegChannels = new();

        foreach (ChannelInfo inputChannel in inputChannels)
        {
            PrefixedUnit? channelUnits = Common.ConvertTextToPrefixedUnit(inputChannel.Unit) ?? throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, Resources.UrecognizedUnitExceptionMessage, inputChannel.Unit, inputChannel.Name));

            EegChannel eegChannel = new(
                //REQUIRED
                name: inputChannel.Name,
                type: ChannelType.EEG,
                units: channelUnits.Value)
            {
                //OPTIONAL
                SamplingFrequency = samplingFrequency,
                //Reference = null,
                //LowCutoff = null,
                //HighCutoff = null,
                //Notch = null,
            };

            eegChannels.Add(eegChannel);
        }

        return eegChannels;
    }
}
