using BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat.MarkerFileEnums;

namespace BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat.Internal;

internal sealed class MarkerFileContent : IMarkerFileContentVer1
{
    public MarkerFileContent(string identificationText, Version version)
    {
        IdentificationText = identificationText;
        Version = version;
    }

    #region IdentificationText
    public string IdentificationText { get; }
    public Version Version { get; }
    #endregion

    #region Common Infos Section
    public Codepage? CodePage { get; set; }

    public string? DataFile { get; set; }
    #endregion

    #region Marker Infos Section
    private IList<MarkerInfo>? _markers;
    public IList<MarkerInfo>? GetMarkers() => _markers;
    public void SetMarkers(IList<MarkerInfo>? markers) => _markers = markers;
    #endregion

    #region InlinedComments
    public MarkerFileInlinedComments InlinedComments { get; } = new MarkerFileInlinedComments();
    #endregion
}
