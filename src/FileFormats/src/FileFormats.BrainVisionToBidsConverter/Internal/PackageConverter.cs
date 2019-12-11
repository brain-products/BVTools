using System.Collections.Generic;
using System.IO;
using BrainVision.Lab.FileFormats.Internal.Converters;
using BrainVision.Lab.FileFormats.PublicDomain.BidsFormat;

namespace BrainVision.Lab.FileFormats.Internal
{
    internal class PackageConverter
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

        public void ConvertBrainVisionFilesToBidsFormatFiles()
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

            SaveBidsAgnosticFiles(_bidsPackage.RootFolder,
                datasetDescription, changesFileContent, readmeFileContent);

            SaveBidsEegModalityFiles(_bidsPackage.RootFolder.SubjectFolder.SessionFolder.EegModalityFolder,
                eegSidecar, eegChannels, eegElectrodes, eegCoordinateSystem, taskEvents);
        }

        private void SaveBidsAgnosticFiles(IRootFolder rootFolder, DatasetDescription datasetDescription, string changesFileContent, string readmeFileContent)
        {
            if (datasetDescription != null)
            {
                if (File.Exists(rootFolder.DatasetDescriptionFilePath))
                    Warnings.Add($"{WarningFileSkipped}{rootFolder.DatasetDescriptionFilePath}");
                else
                    rootFolder.SaveDatasetDescriptionFile(datasetDescription);
            }

            if (changesFileContent != null)
            {
                if (File.Exists(rootFolder.ChangesFilePath))
                    Warnings.Add($"{WarningFileSkipped}{rootFolder.ChangesFilePath}");
                else
                    rootFolder.SaveChangesFile(changesFileContent);
            }

            if (readmeFileContent != null)
            {
                if (File.Exists(rootFolder.ReadmeFilePath))
                    Warnings.Add($"{WarningFileSkipped}{rootFolder.ReadmeFilePath}");
                else
                    rootFolder.SaveReadmeFile(readmeFileContent);
            }
        }

        private void SaveBidsEegModalityFiles(IEegModalityFolder eegModalityFolder, EegSidecar? eegSidecar, EegChannelCollection? eegChannels, EegElectrodeCollection? eegElectrodes, EegCoordinateSystem? eegCoordinateSystem, TaskEventCollection? taskEvents)
        {
            if (eegSidecar != null)
            {
                if (File.Exists(eegModalityFolder.EegSidecarFilePath))
                    Warnings.Add($"{WarningFileOverwritten}{eegModalityFolder.EegSidecarFilePath}");

                eegModalityFolder.SaveEegSidecarFile(eegSidecar);
            }

            if (eegChannels != null)
            {
                if (File.Exists(eegModalityFolder.EegChannelsFilePath))
                    Warnings.Add($"{WarningFileOverwritten}{eegModalityFolder.EegChannelsFilePath}");

                eegModalityFolder.SaveEegChannelsFile(eegChannels);
            }

            if (eegElectrodes != null)
            {
                if (File.Exists(eegModalityFolder.EegElectrodesFilePath))
                    Warnings.Add($"{WarningFileOverwritten}{eegModalityFolder.EegElectrodesFilePath}");

                eegModalityFolder.SaveEegElectrodesFile(eegElectrodes);
            }

            if (eegCoordinateSystem != null)
            {
                if (File.Exists(eegModalityFolder.EegCoordinatesFilePath))
                    Warnings.Add($"{WarningFileOverwritten}{eegModalityFolder.EegCoordinatesFilePath}");

                eegModalityFolder.SaveEegCoordinateSystemFile(eegCoordinateSystem);
            }

            if (taskEvents != null)
            {
                if (File.Exists(eegModalityFolder.TaskEventsFilePath))
                    Warnings.Add($"{WarningFileOverwritten}{eegModalityFolder.TaskEventsFilePath}");

                eegModalityFolder.SaveTaskEventsFile(taskEvents);
            }
        }

        public void CopyBrainVisionFilesToBidsEegModalityFolder()
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
            copiedBvFilesPackage.Save(copiedBvFilesPackage.HeaderFilePath);
        }

        private void CopyFileWithFileOverwrittenWarningIfNecessary(string srcFilePath, string dstFilePath)
        {
            if (File.Exists(dstFilePath))
                Warnings.Add($"{WarningFileOverwritten}{dstFilePath}");

            File.Copy(srcFilePath, dstFilePath, true);
        }

        private static string FormatFileName(string fileName, string appendix, string extention)
            => $"{fileName}{appendix}{extention}";
    }
}
