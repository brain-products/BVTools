using System;
using System.Collections.Generic;

namespace BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat.Internal
{
    internal class HeaderFileContent : IHeaderFileContentVer1
    {
        public HeaderFileContent(string identificationText, Version version)
        {
            IdentificationText = identificationText;
            Version = version;
        }

        #region Default values
        public void UpdateMissingKeysWithDefaultValues()
        {
            UpdateMissingCommonInfosKeysWithDefaultValues();
            UpdateMissingChannelInfosKeysWithDefaultValues();
        }

        private void UpdateMissingCommonInfosKeysWithDefaultValues()
        {
            if (DataType == null)
                DataType = (this as IHeaderFileContentVer1).DefaultDataType;

            if (BinaryFormat == null)
                BinaryFormat = (this as IHeaderFileContentVer1).DefaultBinaryFormat;

            if (SegmentationType == null)
                SegmentationType = (this as IHeaderFileContentVer1).DefaultSegmentationType;

            if (SegmentationType == global::SegmentationType.NOTSEGMENTED)
            {
                if (SegmentDataPoints == null)
                    SegmentDataPoints = (this as IHeaderFileContentVer1).DefaultSegmentDataPoints;
            }

            if (Averaged == null)
                Averaged = (this as IHeaderFileContentVer1).DefaultAveraged;

            if (Averaged.Value == false)
            {
                if (AveragedSegments == null)
                    AveragedSegments = (this as IHeaderFileContentVer1).DefaultAveragedSegments;
            }
        }

        private void UpdateMissingChannelInfosKeysWithDefaultValues()
        {
            if (_channelInfos != null)
            {
                for (int i = 0; i < _channelInfos.Count; ++i)
                {
                    ChannelInfo info = _channelInfos[i];
                    if (info.Resolution == null)
                    {
                        info.Resolution = 1.0;
                        _channelInfos[i] = info;
                    }

                    if (string.IsNullOrEmpty(info.Unit))
                    {
                        info.Unit = Definitions.MicroVolt;
                        _channelInfos[i] = info;
                    }
                }
            }
        }
        #endregion

        #region IdentificationText
        public string IdentificationText { get; }
        public Version Version { get; }
        #endregion

        #region Common Infos Section
        public Codepage? CodePage { get; set; }

        public string? DataFile { get; set; }

        public string? MarkerFile { get; set; }

        public DataFormat? DataFormat { get; set; }

        public DataOrientation? DataOrientation { get; set; }

        public DataType? DataType { get; set; }

        public int? NumberOfChannels { get; set; }

        public double? SamplingInterval { get; set; }

        public SegmentationType? SegmentationType { get; set; }

        public int? SegmentDataPoints { get; set; }

        public bool? Averaged { get; set; }

        public int? AveragedSegments { get; set; }
        #endregion

        #region Binary Infos Section
        public BinaryFormat? BinaryFormat { get; set; }
        #endregion

        #region Channel Infos Section
        private List<ChannelInfo>? _channelInfos = null;
        public List<ChannelInfo>? GetChannelInfos() => _channelInfos;
        public void SetChannelInfos(List<ChannelInfo>? channelInfos) => _channelInfos = channelInfos;
        #endregion

        #region Coordinates Section
        private List<Coordinates>? _channelCoordinates = null;

        /// <summary>
        /// a dictionary key is 0-indexed channel number
        /// </summary>
        public List<Coordinates>? GetChannelCoordinates() => _channelCoordinates;

        /// <summary>
        /// a dictionary key is 0-indexed channel number
        /// </summary>
        public void SetChannelCoordinates(List<Coordinates>? channelCoordinates) => _channelCoordinates = channelCoordinates;
        #endregion

        #region Comment Section
        public string? Comment { get; set; }
        #endregion

        #region InlinedComments
        public HeaderFileInlinedComments InlinedComments { get; } = new HeaderFileInlinedComments();
        #endregion
    }
}
