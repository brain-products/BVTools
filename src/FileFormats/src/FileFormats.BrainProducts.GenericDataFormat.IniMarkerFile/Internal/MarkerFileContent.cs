using System;
using System.Collections.Generic;

namespace BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat.Internal
{
    internal class MarkerFileContent : IMarkerFileContentVer1
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
        private List<MarkerInfo>? _markers;
        public List<MarkerInfo>? GetMarkers() => _markers;
        public void SetMarkers(List<MarkerInfo>? markers) => _markers = markers;
        #endregion

        #region InlinedComments
        public MarkerFileInlinedComments InlinedComments { get; } = new MarkerFileInlinedComments();
        #endregion
    }
}
