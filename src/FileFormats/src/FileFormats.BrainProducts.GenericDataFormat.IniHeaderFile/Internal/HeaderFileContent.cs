using BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat.HeaderFileEnums;

namespace BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat.Internal;

internal sealed class HeaderFileContent : IHeaderFileContentVer1
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
        DataType ??= (this as IHeaderFileContentVer1).DefaultDataType;

        BinaryFormat ??= (this as IHeaderFileContentVer1).DefaultBinaryFormat;

        SegmentationType ??= (this as IHeaderFileContentVer1).DefaultSegmentationType;

        if (SegmentationType == HeaderFileEnums.SegmentationType.NOTSEGMENTED)
        {
            SegmentDataPoints ??= (this as IHeaderFileContentVer1).DefaultSegmentDataPoints;
        }

        Averaged ??= (this as IHeaderFileContentVer1).DefaultAveraged;

        if (Averaged.Value == false)
        {
            AveragedSegments ??= (this as IHeaderFileContentVer1).DefaultAveragedSegments;
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
    private IList<ChannelInfo>? _channelInfos;
    public IList<ChannelInfo>? GetChannelInfos() => _channelInfos;
    public void SetChannelInfos(IList<ChannelInfo>? channelInfos) => _channelInfos = channelInfos;
    #endregion

    #region Coordinates Section
    private IList<Coordinates>? _channelCoordinates;

    /// <summary>
    /// a dictionary key is 0-indexed channel number
    /// </summary>
    public IList<Coordinates>? GetChannelCoordinates() => _channelCoordinates;

    /// <summary>
    /// a dictionary key is 0-indexed channel number
    /// </summary>
    public void SetChannelCoordinates(IList<Coordinates>? channelCoordinates) => _channelCoordinates = channelCoordinates;
    #endregion

    #region Comment Section
    public string? Comment { get; set; }
    #endregion

    #region InlinedComments
    public HeaderFileInlinedComments InlinedComments { get; } = new HeaderFileInlinedComments();
    #endregion
}
