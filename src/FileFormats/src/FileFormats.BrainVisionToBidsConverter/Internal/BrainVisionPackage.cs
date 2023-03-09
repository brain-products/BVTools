using System.Diagnostics.CodeAnalysis;
using BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat;
using BrainVision.Lab.FileFormats.Properties;

namespace BrainVision.Lab.FileFormats.Internal;

/// <summary>
/// Package keeps paths and content of all Brain Vision files in one place.
/// </summary>
internal sealed class BrainVisionPackage
{
    private string? _rawDataFilePath;
    private string? _headerFilePath;
    private string? _markerFilePath;
    private IHeaderFileContentVer1? _headerFileContent;

    public string RawDataFilePath => _rawDataFilePath!;
    public string HeaderFilePath => _headerFilePath!;
    public string? MarkerFilePath => _markerFilePath;

    public IHeaderFileContentVer1 HeaderFileContent { get => _headerFileContent!; set => _headerFileContent = value; }
    public IMarkerFileContentVer1? MarkerFileContent { get; set; }

    private BrainVisionPackage() { }

    private async Task LoadContentAsync(string brainVisionHeaderFilePath)
    {
        string headerFullPath = Path.GetFullPath(brainVisionHeaderFilePath);
        HeaderFileContent = await LoadHeaderFileAsync(headerFullPath).ConfigureAwait(false);
        _headerFilePath = headerFullPath;

        string? markerFullPath = GetMarkerFileFullPath(headerFullPath, HeaderFileContent.MarkerFile);
        if (markerFullPath != null)
        {
            MarkerFileContent = await LoadMarkerFileAsync(markerFullPath).ConfigureAwait(false);
            _markerFilePath = markerFullPath;
        }

        string dataFullPath = GetDataFileFullPath(headerFullPath, HeaderFileContent.DataFile!);
        LoadRawDataFile(dataFullPath, out _rawDataFilePath);
    }

    public static BrainVisionPackage Copy(BrainVisionPackage srcPackage)
        => (BrainVisionPackage)srcPackage.MemberwiseClone();

    public static async Task<BrainVisionPackage> LoadAsync(string brainVisionHeaderFilePath)
    {
        BrainVisionPackage brainVisionFilesContent = new();
        await brainVisionFilesContent.LoadContentAsync(brainVisionHeaderFilePath).ConfigureAwait(false);
        return brainVisionFilesContent;
    }

    private static async Task<IHeaderFileContentVer1> LoadHeaderFileAsync(string path)
    {
        try
        {
#pragma warning disable CA2000 // Dispose objects before losing scope
            IHeaderFile headerFile = HeaderFileFactory.OpenForRead(path);
#pragma warning restore CA2000 // Dispose objects before losing scope
            await using (headerFile.ConfigureAwait(false))
            {
                return await headerFile.LoadVer1Async().ConfigureAwait(false);
            }
        }
        catch (Exception e)
        {
            throw new InvalidOperationException($"{Resources.FailedToLoadBrainVisionHeaderFileExceptionMessage} {e.Message}", e);
        }
    }

    private static async Task<IMarkerFileContentVer1> LoadMarkerFileAsync(string path)
    {
        try
        {
#pragma warning disable CA2000 // Dispose objects before losing scope
            IMarkerFile markerFile = MarkerFileFactory.OpenForRead(path);
#pragma warning restore CA2000 // Dispose objects before losing scope
            await using (markerFile.ConfigureAwait(false))
            {
                return await markerFile.LoadVer1Async().ConfigureAwait(false);
            }
        }
        catch (Exception e)
        {
            throw new InvalidOperationException($"{Resources.FailedToLoadBrainVisionMarkerFileExceptionMessage} {e.Message}", e);
        }
    }

    private static void LoadRawDataFile(string path, out string rawDataFilePath)
    {
        if (!File.Exists(path))
            throw new InvalidOperationException($"{Resources.FailedToFindBrainVisionRawDataFileExceptionMessage} {path}");

        rawDataFilePath = path;
    }

    private static string GetDataFileFullPath(string headerFullPath, string dataFileName)
    {
        string dataFullPath = Path.Combine(Path.GetPathRoot(headerFullPath)!, Path.GetDirectoryName(headerFullPath)!, dataFileName);
        return dataFullPath;
    }

    [return: NotNullIfNotNull(nameof(markerFileName))]
    private static string? GetMarkerFileFullPath(string headerFullPath, string? markerFileName)
    {
        if (markerFileName == null)
            return null;

        string markerFullPath = Path.Combine(Path.GetPathRoot(headerFullPath)!, Path.GetDirectoryName(headerFullPath)!, markerFileName);
        return markerFullPath;
    }

    public async Task SaveAsync(string brainVisionHeaderFilePath)
    {
        string headerFullPath = Path.GetFullPath(brainVisionHeaderFilePath);
#pragma warning disable CA2000 // Dispose objects before losing scope
        IHeaderFile headerFile = HeaderFileFactory.OpenForReadWrite(headerFullPath);
#pragma warning restore CA2000 // Dispose objects before losing scope
        await using (headerFile.ConfigureAwait(false))
        {
            await headerFile.SaveVer1Async(HeaderFileContent).ConfigureAwait(false);
        }

        if (MarkerFileContent != null)
        {
            string markerFullPath = GetMarkerFileFullPath(headerFullPath, HeaderFileContent.MarkerFile!);
#pragma warning disable CA2000 // Dispose objects before losing scope
            IMarkerFile markerFile = MarkerFileFactory.OpenForReadWrite(markerFullPath);
#pragma warning restore CA2000 // Dispose objects before losing scope
            await using (markerFile.ConfigureAwait(false))
            {
                await markerFile.SaveVer1Async(MarkerFileContent).ConfigureAwait(false);
            }
        }
    }

    public void UpdateMissingKeysWithDefaultValues()
        => HeaderFileContent.UpdateMissingKeysWithDefaultValues();

    public void SetPathsAndUpdateFileReferencesInBvFiles(string rawDataFilePath, string headerFilePath, string markerFilePath)
    {
        _rawDataFilePath = rawDataFilePath;
        _headerFilePath = headerFilePath;
        _markerFilePath = markerFilePath;

        string rawDataFileFileName = Path.GetFileName(_rawDataFilePath);
        string markerFileFileName = Path.GetFileName(_markerFilePath);

        UpdateFileReferencesInBvFiles(rawDataFileFileName, markerFileFileName);
    }

    public void UpdateFileReferencesInBvFiles(string rawDataFileFileName, string markerFileFileName)
    {
        HeaderFileContent.DataFile = rawDataFileFileName;
        HeaderFileContent.MarkerFile = markerFileFileName;
        if (MarkerFileContent != null)
            MarkerFileContent.DataFile = rawDataFileFileName;
    }
}
