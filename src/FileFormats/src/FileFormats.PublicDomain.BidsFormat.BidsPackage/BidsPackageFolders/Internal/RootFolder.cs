using BrainVision.Lab.FileFormats.PublicDomain.BidsFormat.Internal.Writers;

namespace BrainVision.Lab.FileFormats.PublicDomain.BidsFormat.Internal;

internal sealed class RootFolder : IRootFolder
{
    private readonly DatasetInfo _datasetInfo;
    internal RootFolder(DatasetInfo datasetInfo)
    {
        _datasetInfo = datasetInfo;
        SubjectFolder = new SubjectFolder(this, datasetInfo);
    }

    #region Properties
    public string FolderPath => _datasetInfo.Root;
    public ISubjectFolder SubjectFolder { get; }
    #endregion

    #region File Names
    public string DatasetDescriptionFileName { get; } = "dataset_description.json";
    public string ReadmeFileName { get; } = "README";
    public string ChangesFileName { get; } = "CHANGES";
    #endregion

    #region File Paths
    public string DatasetDescriptionFilePath => Path.Combine(FolderPath, DatasetDescriptionFileName);
    public string ReadmeFilePath => Path.Combine(FolderPath, ReadmeFileName);
    public string ChangesFilePath => Path.Combine(FolderPath, ChangesFileName);
    #endregion

    #region Save
    public async Task SaveDatasetDescriptionFileAsync(DatasetDescription datasetDescription)
        => await SaveDatasetDescriptionFileAsync(DatasetDescriptionFilePath, datasetDescription).ConfigureAwait(false);

    public async Task SaveReadmeFileAsync(string fileContent)
        => await SaveReadmeFileAsync(ReadmeFilePath, fileContent).ConfigureAwait(false);

    public async Task SaveChangesFileAsync(string fileContent)
        => await SaveChangesFileAsync(ChangesFilePath, fileContent).ConfigureAwait(false);
    #endregion

    #region Static Save
    public static async Task SaveDatasetDescriptionFileAsync(string filePath, DatasetDescription datasetDescription)
    {
        DirectoryExt.CreateDirectory(Path.GetDirectoryName(filePath));
        await DatasetDescriptionWriter.SaveAsync(filePath, datasetDescription).ConfigureAwait(false);
    }

    public static async Task SaveReadmeFileAsync(string filePath, string fileContent)
    {
        DirectoryExt.CreateDirectory(Path.GetDirectoryName(filePath));
        await ReadmeWriter.SaveAsync(filePath, fileContent).ConfigureAwait(false);
    }

    public static async Task SaveChangesFileAsync(string filePath, string fileContent)
    {
        DirectoryExt.CreateDirectory(Path.GetDirectoryName(filePath));
        await ChangesWriter.SaveAsync(filePath, fileContent).ConfigureAwait(false);
    }
    #endregion
}
