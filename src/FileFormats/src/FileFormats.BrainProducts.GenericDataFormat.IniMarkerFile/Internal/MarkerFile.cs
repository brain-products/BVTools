using BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat.Properties;

namespace BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat.Internal;

internal sealed class MarkerFile : IMarkerFile
{
    #region variables
    private readonly FileStream _file;
    private readonly bool _newFileBeingCreated;
    #endregion

    public MarkerFile(string path, FileMode mode, FileAccess access, FileShare share)
    {
        bool openingExistingFile = File.Exists(path);

        _file = new FileStream(path, mode, access, share);

        _newFileBeingCreated = !openingExistingFile || mode == FileMode.Truncate;
        if (_newFileBeingCreated)
        {
            Version = new Version(1, 0);

            FileSaver saver = new(_file);
            saver.SaveEmpty();
        }
        else
        {
            ThrowExceptionIfFileExtensionNotRecognized(path);

            FileLoader fileLoader = new(_file);
            Version = fileLoader.ReadVersion();
        }
    }

    private static void ThrowExceptionIfFileExtensionNotRecognized(string path)
    {
        string fileExtension = Path.GetExtension(path);
        const string validExtension = "vmrk";
        bool isExtensionValid = fileExtension == $".{validExtension}";
        if (!isExtensionValid)
            throw new InvalidMarkerFileFormatException(0, $"{Resources.UnrecognizedFileExtension} {validExtension}");
    }

    #region load/save
    public Version Version { get; }

    public async Task<IMarkerFileContentVer1> LoadVer1Async()
    {
        if (_newFileBeingCreated)
        {
            MarkerFileContent content = new(Definitions.IdentificationText, new Version(1, 0));
            return content;
        }
        else
        {
            FileLoader fileLoader = new(_file);
            return await fileLoader.LoadVer1Async().ConfigureAwait(false);
        }
    }

    public async Task SaveVer1Async(IMarkerFileContentVer1 header)
    {
        FileSaver fileSaver = new(_file);
        await fileSaver.SaveVer1Async(header).ConfigureAwait(false);
    }
    #endregion

    #region dispose
    public ValueTask DisposeAsync() => _file.DisposeAsync();
    #endregion
}
