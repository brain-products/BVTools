using BrainVision.Lab.FileFormats.Internal.Converters;
using BrainVision.Lab.FileFormats.PublicDomain.BidsFormat;

namespace BrainVision.Lab.FileFormats.Internal;

internal sealed class PackageConverter
{
    private const string WarningFileOverwritten = "File already exists. It has been overwritten: ";
    private const string WarningFileSkipped = "File already exists and will not be overwritten or modified: ";
    private readonly BrainVisionPackage _bvPackage;
    private readonly IBidsPackage _bidsPackage;
    private readonly CustomizationInfo _info;

    public PackageConverter(BrainVisionPackage bvPackage, IBidsPackage bidsPackage, CustomizationInfo info)
    {
        _bvPackage = bvPackage;
        _bidsPackage = bidsPackage;
        _info = info;
    }

    public List<string> Warnings { get; } = new List<string>();

    public async Task ConvertBrainVisionFilesToBidsFormatFilesAsync()
    {
        DatasetDescription datasetDescription = DatasetDescriptionConverter.Collect(_info);
        string changesFileContent = ChangesConverter.Collect();
        string readmeFileContent = string.Empty;

        string actualTaskName = _bidsPackage.Task; // may be auto-generated, in this case will differ from info.TaskName
        EegSidecar eegSidecar = EegSidecarConverter.Collect(_bvPackage, _info, actualTaskName);
        EegChannelCollection? eegChannels = EegChannelsConverter.Collect(_bvPackage);
        EegElectrodeCollection? eegElectrodes = EegElectrodesConverter.Collect(_bvPackage);
        EegCoordinateSystem? eegCoordinateSystem = eegElectrodes == null ? null : EegCoordinateSystemConverter.Collect(); // if electrodes not present, no sense to create coordinates
        TaskEventCollection? taskEvents = TaskEventsConverter.Collect(_bvPackage);

        await SaveBidsAgnosticFilesAsync(_bidsPackage.RootFolder,
            datasetDescription, changesFileContent, readmeFileContent).ConfigureAwait(false);

        await SaveBidsEegModalityFilesAsync(_bidsPackage.RootFolder.SubjectFolder.SessionFolder.EegModalityFolder,
            eegSidecar, eegChannels, eegElectrodes, eegCoordinateSystem, taskEvents).ConfigureAwait(false);
    }

    private async Task SaveBidsAgnosticFilesAsync(IRootFolder rootFolder, DatasetDescription datasetDescription, string changesFileContent, string readmeFileContent)
    {
        if (datasetDescription != null)
        {
            if (File.Exists(rootFolder.DatasetDescriptionFilePath))
                Warnings.Add($"{WarningFileSkipped}{rootFolder.DatasetDescriptionFilePath}");
            else
                await rootFolder.SaveDatasetDescriptionFileAsync(datasetDescription).ConfigureAwait(false);
        }

        if (changesFileContent != null)
        {
            if (File.Exists(rootFolder.ChangesFilePath))
                Warnings.Add($"{WarningFileSkipped}{rootFolder.ChangesFilePath}");
            else
                await rootFolder.SaveChangesFileAsync(changesFileContent).ConfigureAwait(false);
        }

        if (readmeFileContent != null)
        {
            if (File.Exists(rootFolder.ReadmeFilePath))
                Warnings.Add($"{WarningFileSkipped}{rootFolder.ReadmeFilePath}");
            else
                await rootFolder.SaveReadmeFileAsync(readmeFileContent).ConfigureAwait(false);
        }
    }

    private async Task SaveBidsEegModalityFilesAsync(IEegModalityFolder eegModalityFolder, EegSidecar? eegSidecar, EegChannelCollection? eegChannels, EegElectrodeCollection? eegElectrodes, EegCoordinateSystem? eegCoordinateSystem, TaskEventCollection? taskEvents)
    {
        if (eegSidecar != null)
        {
            if (File.Exists(eegModalityFolder.EegSidecarFilePath))
                Warnings.Add($"{WarningFileOverwritten}{eegModalityFolder.EegSidecarFilePath}");

            await eegModalityFolder.SaveEegSidecarFileAsync(eegSidecar).ConfigureAwait(false);
        }

        if (eegChannels != null)
        {
            if (File.Exists(eegModalityFolder.EegChannelsFilePath))
                Warnings.Add($"{WarningFileOverwritten}{eegModalityFolder.EegChannelsFilePath}");

            await eegModalityFolder.SaveEegChannelsFileAsync(eegChannels).ConfigureAwait(false);
        }

        if (eegElectrodes != null)
        {
            if (File.Exists(eegModalityFolder.EegElectrodesFilePath))
                Warnings.Add($"{WarningFileOverwritten}{eegModalityFolder.EegElectrodesFilePath}");

            await eegModalityFolder.SaveEegElectrodesFileAsync(eegElectrodes).ConfigureAwait(false);
        }

        if (eegCoordinateSystem != null)
        {
            if (File.Exists(eegModalityFolder.EegCoordinatesFilePath))
                Warnings.Add($"{WarningFileOverwritten}{eegModalityFolder.EegCoordinatesFilePath}");

            await eegModalityFolder.SaveEegCoordinateSystemFileAsync(eegCoordinateSystem).ConfigureAwait(false);
        }

        if (taskEvents != null)
        {
            if (File.Exists(eegModalityFolder.TaskEventsFilePath))
                Warnings.Add($"{WarningFileOverwritten}{eegModalityFolder.TaskEventsFilePath}");

            await eegModalityFolder.SaveTaskEventsFileAsync(taskEvents).ConfigureAwait(false);
        }
    }

    public async Task CopyBrainVisionFilesToBidsEegModalityFolderAsync()
    {
        string bidsEegModalityFolderPath = _bidsPackage.RootFolder.SubjectFolder.SessionFolder.EegModalityFolder.FolderPath;
        string fileNamePattern = _bidsPackage.FileNamePattern;

        const string appendix = "_eeg";
        string rawDataFileName = FormatFileName(fileNamePattern, appendix, Path.GetExtension(_bvPackage.RawDataFilePath));
        string headerFileName = FormatFileName(fileNamePattern, appendix, Path.GetExtension(_bvPackage.HeaderFilePath));
        string markerFileName = FormatFileName(fileNamePattern, appendix, Path.GetExtension(_bvPackage.MarkerFilePath));

        string rawDataFilePath = Path.Combine(bidsEegModalityFolderPath, rawDataFileName);
        string headerFilePath = Path.Combine(bidsEegModalityFolderPath, headerFileName);
        string markerFilePath = Path.Combine(bidsEegModalityFolderPath, markerFileName);

        Directory.CreateDirectory(bidsEegModalityFolderPath);
        CopyFileWithFileOverwrittenWarningIfNecessary(_bvPackage.RawDataFilePath, rawDataFilePath);
        CopyFileWithFileOverwrittenWarningIfNecessary(_bvPackage.HeaderFilePath, headerFilePath);
        if (_bvPackage.MarkerFilePath != null)
            CopyFileWithFileOverwrittenWarningIfNecessary(_bvPackage.MarkerFilePath, markerFilePath);

        BrainVisionPackage copiedBvFilesPackage = BrainVisionPackage.Copy(_bvPackage);
        copiedBvFilesPackage.SetPathsAndUpdateFileReferencesInBvFiles(rawDataFilePath, headerFilePath, markerFilePath);
        await copiedBvFilesPackage.SaveAsync(copiedBvFilesPackage.HeaderFilePath).ConfigureAwait(false);
    }

    private void CopyFileWithFileOverwrittenWarningIfNecessary(string srcFilePath, string dstFilePath)
    {
        if (File.Exists(dstFilePath))
            Warnings.Add($"{WarningFileOverwritten}{dstFilePath}");

        File.Copy(srcFilePath, dstFilePath, true);
    }

    private static string FormatFileName(string? fileName, string? appendix, string? extention)
        => $"{fileName}{appendix}{extention}";
}
