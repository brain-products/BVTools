using System;

namespace BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat
{
    public interface IHeaderFile : IDisposable
    {
        Version Version { get; }

        IHeaderFileContentVer1 LoadVer1();
        void SaveVer1(IHeaderFileContentVer1 header);
    }
}
