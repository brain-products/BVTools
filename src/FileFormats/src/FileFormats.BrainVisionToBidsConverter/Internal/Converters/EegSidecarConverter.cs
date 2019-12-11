using System.Collections.Generic;
using BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat;
using BrainVision.Lab.FileFormats.PublicDomain.BidsFormat;

namespace BrainVision.Lab.FileFormats.Internal.Converters
{
    internal static class EegSidecarConverter
    {
        public static EegSidecar Collect(BrainVisionPackage filesContent, CustomizationInfo info, string actualTaskName)
        {
            IHeaderFileContentVer1 headerContent = filesContent.HeaderFileContent;
            List<ChannelInfo>? channelInfos = headerContent.GetChannelInfos();

            EegSidecar sidecar = new EegSidecar(
                //REQUIRED Generic
                actualTaskName,
                
                //REQUIRED EEG
                info.EEGReference,
                Common.GetSamplingFrequencyFromSamplingInterval(headerContent.SamplingInterval!.Value), // SamplingInterval is in µs
                info.PowerLineFrequency,
                Defs.NotAvailable)
            {
                #region Generic
                //RECOMMENDED
                Manufacturer = info.Manufacturer,
                #endregion

                #region EEG
                //RECOMMENDED
                EEGChannelCount = channelInfos == null ? 0 : channelInfos.Count,
                ECGChannelCount = 0,
                EMGChannelCount = 0,
                EOGChannelCount = 0,
                MiscChannelCount = 0,
                TriggerChannelCount = 0,
                //RecordingDuration = 0,
                RecordingType = headerContent.Averaged!.Value ? EegSidecar.Recordingtype.epoched : EegSidecar.Recordingtype.continuous,
                EpochLength = headerContent.Averaged.Value ? ((headerContent.SegmentDataPoints!.Value * headerContent.SamplingInterval.Value) / 1000000.0) as double?: null,
                #endregion
            };

            return sidecar;
        }
    }
}
