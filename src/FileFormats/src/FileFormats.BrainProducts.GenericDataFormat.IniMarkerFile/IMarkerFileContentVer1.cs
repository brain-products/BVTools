using BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat.MarkerFileEnums;

namespace BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat;

/// <summary>
/// All property values equal to null means that a specific key does not exist in a file/shall not be saved to file
/// </summary>
public interface IMarkerFileContentVer1
{
    #region IdentificationText
    string IdentificationText { get; }
    Version Version { get; }
    #endregion

    #region Common Infos Section
    Codepage? CodePage { get; set; }

    string? DataFile { get; set; }

    #endregion

    #region Marker Infos Section
    IList<MarkerInfo>? GetMarkers();

    void SetMarkers(IList<MarkerInfo>? markers);
    #endregion

    #region InlinedComments
    MarkerFileInlinedComments InlinedComments { get; }
    #endregion
}
