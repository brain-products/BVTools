using BrainVision.Lab.FileFormats.PublicDomain.BidsFormat.Internal.Writers;

namespace BrainVision.Lab.FileFormats.PublicDomain.BidsFormat.Internal;

internal sealed class EegModalityFolder : IEegModalityFolder
{
    private readonly DatasetInfo _datasetInfo;

    internal EegModalityFolder(SessionFolder parent, DatasetInfo datasetInfo)
    {
        _datasetInfo = datasetInfo;
        Parent = parent;
    }

    #region Properties
    public ISessionFolder Parent { get; }
    public string FolderName { get; } = "eeg";
    public string FolderPath => Path.Combine(Parent.FolderPath, FolderName);
    #endregion

    #region File Names
    public string EegSidecarFileName => $"{_datasetInfo.FileNamePattern}_eeg.json";
    public string EegChannelsFileName => $"{_datasetInfo.FileNamePattern}_channels.tsv";
    public string EegElectrodesFileName => $"{_datasetInfo.FileNamePattern}_electrodes.tsv";
    public string EegCoordinatesFileName => $"{_datasetInfo.FileNamePattern}_coordsystem.json";
    public string TaskEventsFileName => $"{_datasetInfo.FileNamePattern}_events.tsv";
    #endregion

    #region File Paths
    public string EegSidecarFilePath => Path.Combine(FolderPath, EegSidecarFileName);

    public string EegChannelsFilePath => Path.Combine(FolderPath, EegChannelsFileName);

    public string EegElectrodesFilePath => Path.Combine(FolderPath, EegElectrodesFileName);

    public string EegCoordinatesFilePath => Path.Combine(FolderPath, EegCoordinatesFileName);

    public string TaskEventsFilePath => Path.Combine(FolderPath, TaskEventsFileName);
    #endregion

    #region Save
    public async Task SaveEegSidecarFileAsync(EegSidecar sidecar)
        => await SaveEegSidecarFileAsync(EegSidecarFilePath, sidecar).ConfigureAwait(false);

    public async Task SaveEegChannelsFileAsync(EegChannelCollection channels)
        => await SaveEegChannelsFileAsync(EegChannelsFilePath, channels).ConfigureAwait(false);

    public async Task SaveEegElectrodesFileAsync(EegElectrodeCollection electrodes)
        => await SaveEegElectrodesFileAsync(EegElectrodesFilePath, electrodes).ConfigureAwait(false);

    public async Task SaveEegCoordinateSystemFileAsync(EegCoordinateSystem eegCoordinateSystem)
        => await SaveEegCoordinateSystemFileAsync(EegCoordinatesFilePath, eegCoordinateSystem).ConfigureAwait(false);

    public async Task SaveTaskEventsFileAsync(TaskEventCollection taskEvents)
        => await SaveTaskEventsFileAsync(TaskEventsFilePath, taskEvents).ConfigureAwait(false);
    #endregion

    #region Static Save
    public static async Task SaveEegSidecarFileAsync(string filePath, EegSidecar sidecar)
    {
        DirectoryExt.CreateDirectory(Path.GetDirectoryName(filePath));
        await EegSidecarWriter.SaveAsync(filePath, sidecar).ConfigureAwait(false);
    }

    public static async Task SaveEegChannelsFileAsync(string filePath, EegChannelCollection channels)
    {
        DirectoryExt.CreateDirectory(Path.GetDirectoryName(filePath));
        await EegChannelsWriter.SaveAsync(filePath, channels).ConfigureAwait(false);
    }

    public static async Task SaveEegElectrodesFileAsync(string filePath, EegElectrodeCollection electrodes)
    {
        DirectoryExt.CreateDirectory(Path.GetDirectoryName(filePath));
        await EegElectrodesWriter.SaveAsync(filePath, electrodes).ConfigureAwait(false);
    }

    public static async Task SaveEegCoordinateSystemFileAsync(string filePath, EegCoordinateSystem eegCoordinateSystem)
    {
        DirectoryExt.CreateDirectory(Path.GetDirectoryName(filePath));
        await EegCoordinateSystemWriter.SaveAsync(filePath, eegCoordinateSystem).ConfigureAwait(false);
    }

    public static async Task SaveTaskEventsFileAsync(string filePath, TaskEventCollection taskEvents)
    {
        DirectoryExt.CreateDirectory(Path.GetDirectoryName(filePath));
        await TaskEventsWriter.SaveAsync(filePath, taskEvents).ConfigureAwait(false);
    }
    #endregion
}
