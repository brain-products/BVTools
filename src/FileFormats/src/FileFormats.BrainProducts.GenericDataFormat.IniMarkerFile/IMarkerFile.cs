using System;

namespace BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat
{
    public interface IMarkerFile : IDisposable
    {
        Version Version { get; }

        IMarkerFileContentVer1 LoadVer1();
        void SaveVer1(IMarkerFileContentVer1 header);
    }
}
