namespace BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat;

public interface IHeaderFile : IAsyncDisposable
{
    Version Version { get; }

    Task<IHeaderFileContentVer1> LoadVer1Async();
    Task SaveVer1Async(IHeaderFileContentVer1 header);
}
