using System.IO;
using BrainVision.Lab.FileFormats.PublicDomain.BidsFormat.Internal.Writers;

namespace BrainVision.Lab.FileFormats.PublicDomain.BidsFormat.Internal
{
    internal class EegModalityFolder : IEegModalityFolder
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
        public void SaveEegSidecarFile(EegSidecar sidecar)
            => SaveEegSidecarFile(EegSidecarFilePath, sidecar);

        public void SaveEegChannelsFile(EegChannelCollection channels)
            => SaveEegChannelsFile(EegChannelsFilePath, channels);

        public void SaveEegElectrodesFile(EegElectrodeCollection electrodes)
            => SaveEegElectrodesFile(EegElectrodesFilePath, electrodes);

        public void SaveEegCoordinateSystemFile(EegCoordinateSystem eegCoordinateSystem)
            => SaveEegCoordinateSystemFile(EegCoordinatesFilePath, eegCoordinateSystem);

        public void SaveTaskEventsFile(TaskEventCollection taskEvents)
            => SaveTaskEventsFile(TaskEventsFilePath, taskEvents);
        #endregion

        #region Static Save
        public static void SaveEegSidecarFile(string filePath, EegSidecar sidecar)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            EegSidecarWriter.Save(filePath, sidecar);
        }

        public static void SaveEegChannelsFile(string filePath, EegChannelCollection channels)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            EegChannelsWriter.Save(filePath, channels);
        }

        public static void SaveEegElectrodesFile(string filePath, EegElectrodeCollection electrodes)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            EegElectrodesWriter.Save(filePath, electrodes);
        }

        public static void SaveEegCoordinateSystemFile(string filePath, EegCoordinateSystem eegCoordinateSystem)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            EegCoordinateSystemWriter.Save(filePath, eegCoordinateSystem);
        }

        public static void SaveTaskEventsFile(string filePath, TaskEventCollection taskEvents)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            TaskEventsWriter.Save(filePath, taskEvents);
        }
        #endregion
    }
}
