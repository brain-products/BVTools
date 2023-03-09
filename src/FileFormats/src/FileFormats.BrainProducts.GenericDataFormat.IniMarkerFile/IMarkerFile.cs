namespace BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat;

public interface IMarkerFile : IAsyncDisposable
{
    Version Version { get; }

    Task<IMarkerFileContentVer1> LoadVer1Async();
    Task SaveVer1Async(IMarkerFileContentVer1 header);
}
