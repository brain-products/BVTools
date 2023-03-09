using BrainVision.Lab.FileFormats.PublicDomain.BidsFormat.Internal;

namespace BrainVision.Lab.FileFormats.PublicDomain.BidsFormat;

/// <summary>
/// The class allows direct access to bids files without creating a bids package.
/// This may be useful when a file needs to be created/read as a stand alone file.
/// </summary>
public static class BidsPackageFiles
{
    public const string SubjectFolderPrefix = SubjectFolder.FolderPrefix;
    public const string SessionFolderPrefix = SessionFolder.FolderPrefix;

    #region RootFolder Save
    public static async Task SaveDatasetDescriptionFileAsync(string filePath, DatasetDescription datasetDescription)
        => await RootFolder.SaveDatasetDescriptionFileAsync(filePath, datasetDescription).ConfigureAwait(false);

    public static async Task SaveReadmeFileAsync(string filePath, string fileContent)
        => await RootFolder.SaveReadmeFileAsync(filePath, fileContent).ConfigureAwait(false);

    public static async Task SaveChangesFileAsync(string filePath, string fileContent)
        => await RootFolder.SaveChangesFileAsync(filePath, fileContent).ConfigureAwait(false);
    #endregion

    #region EegModalityFolder Save
    public static async Task SaveEegSidecarFileAsync(string filePath, EegSidecar sidecar)
        => await EegModalityFolder.SaveEegSidecarFileAsync(filePath, sidecar).ConfigureAwait(false);

    public static async Task SaveEegChannelsFileAsync(string filePath, EegChannelCollection channels)
        => await EegModalityFolder.SaveEegChannelsFileAsync(filePath, channels).ConfigureAwait(false);

    public static async Task SaveEegElectrodesFileAsync(string filePath, EegElectrodeCollection electrodes)
        => await EegModalityFolder.SaveEegElectrodesFileAsync(filePath, electrodes).ConfigureAwait(false);

    public static async Task SaveEegCoordinateSystemFileAsync(string filePath, EegCoordinateSystem eegCoordinateSystem)
        => await EegModalityFolder.SaveEegCoordinateSystemFileAsync(filePath, eegCoordinateSystem).ConfigureAwait(false);

    public static async Task SaveTaskEventsFileAsync(string filePath, TaskEventCollection taskEvents)
        => await EegModalityFolder.SaveTaskEventsFileAsync(filePath, taskEvents).ConfigureAwait(false);
    #endregion
}
