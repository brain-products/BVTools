using System;
using System.Collections.Generic;

namespace BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat
{
    /// <summary>
    /// If a property value is null, it means that:
    /// <list type="bullet">
    /// <item><description>when loading: a corresponding key does not exist in a file</description></item>
    /// <item><description>when saving: a corresponding key will not be saved to a file</description></item>
    /// </list>
    /// </summary>
    public interface IHeaderFileContentVer1
    {
        #region Default values
        DataType DefaultDataType => global::DataType.TIMEDOMAIN;
        bool DefaultAveraged => false;
        BinaryFormat DefaultBinaryFormat => global::BinaryFormat.INT_16;
        SegmentationType DefaultSegmentationType => global::SegmentationType.NOTSEGMENTED;
        int DefaultAveragedSegments => 0;
        int DefaultSegmentDataPoints => 0;

        void UpdateMissingKeysWithDefaultValues();
        #endregion

        #region IdentificationText
        string IdentificationText { get; }
        Version Version { get; }
        #endregion

        #region Common Infos Section
        /// <summary>
        /// Mandatory
        /// </summary>
        Codepage? CodePage { get; set; }

        /// <summary>
        /// Mandatory
        /// </summary>
        string? DataFile { get; set; }

        /// <summary>
        /// Optional
        /// </summary>
        string? MarkerFile { get; set; }

        /// <summary>
        /// Mandatory
        /// </summary>
        DataFormat? DataFormat { get; set; }

        /// <summary>
        /// Mandatory
        /// </summary>
        DataOrientation? DataOrientation { get; set; }

        /// <summary>
        /// Optional, if it doesn't exist, it will be assumed as "TimeDomain"
        /// </summary>
        DataType? DataType { get; set; }

        /// <summary>
        /// Mandatory
        /// </summary>
        int? NumberOfChannels { get; set; }

        /// <summary>
        /// Mandatory
        /// </summary>
        double? SamplingInterval { get; set; }

        /// <summary>
        /// Optional, if it doesn't exist, it will be assumed as "false"
        /// </summary>
        bool? Averaged { get; set; }

        /// <summary>
        /// Mandatory if Averaged == true
        /// Optional  if Averaged == false, if it doesn't exist, it will be assumed as "NotSegmented"
        /// </summary>
        SegmentationType? SegmentationType { get; set; }

        /// <summary>
        /// Mandatory if SegmentationType != NotSegmented
        /// </summary>
        int? SegmentDataPoints { get; set; }

        /// <summary>
        /// Mandatory if Averaged == true
        /// </summary>
        int? AveragedSegments { get; set; }
        #endregion

        #region Binary Infos Section
        /// <summary>
        /// Mandatory
        /// </summary>
        BinaryFormat? BinaryFormat { get; set; }
        #endregion

        #region Channel Infos Section
        /// <summary>
        /// Mandatory
        /// </summary>
        List<ChannelInfo>? GetChannelInfos();
        /// <summary>
        /// Mandatory
        /// </summary>
        void SetChannelInfos(List<ChannelInfo>? channelInfos);
        #endregion

        #region Coordinates Section
        /// <summary>
        /// Optional
        /// A dictionary key is 0-indexed channel number
        /// </summary>
        List<Coordinates>? GetChannelCoordinates();
        /// <summary>
        /// Optional
        /// A dictionary key is 0-indexed channel number
        /// </summary>
        void SetChannelCoordinates(List<Coordinates>? channelCoordinates);
        #endregion

        #region Comment Section
        /// <summary>
        /// Optional
        /// </summary>
        string? Comment { get; set; }
        #endregion

        #region InlinedComments
        /// <summary>
        /// Optional
        /// </summary>
        HeaderFileInlinedComments InlinedComments { get; }
        #endregion
    }
}
