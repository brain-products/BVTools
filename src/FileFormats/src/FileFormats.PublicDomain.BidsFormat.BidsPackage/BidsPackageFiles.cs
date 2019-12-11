using BrainVision.Lab.FileFormats.PublicDomain.BidsFormat.Internal;

namespace BrainVision.Lab.FileFormats.PublicDomain.BidsFormat
{
    /// <summary>
    /// The class allows direct access to bids files without creating a bids package.
    /// This may be useful when a file needs to be created/read as a stand alone file.
    /// </summary>
    public static class BidsPackageFiles
    {
        public const string SubjectFolderPrefix = SubjectFolder.FolderPrefix;
        public const string SessionFolderPrefix = SessionFolder.FolderPrefix;

        #region RootFolder Save
        public static void SaveDatasetDescriptionFile(string filePath, DatasetDescription datasetDescription)
            => RootFolder.SaveDatasetDescriptionFile(filePath, datasetDescription);

        public static void SaveReadmeFile(string filePath, string fileContent)
            => RootFolder.SaveReadmeFile(filePath, fileContent);

        public static void SaveChangesFile(string filePath, string fileContent)
            => RootFolder.SaveChangesFile(filePath, fileContent);
        #endregion

        #region EegModalityFolder Save
        public static void SaveEegSidecarFile(string filePath, EegSidecar sidecar)
            => EegModalityFolder.SaveEegSidecarFile(filePath, sidecar);

        public static void SaveEegChannelsFile(string filePath, EegChannelCollection channels)
            => EegModalityFolder.SaveEegChannelsFile(filePath, channels);

        public static void SaveEegElectrodesFile(string filePath, EegElectrodeCollection electrodes)
            => EegModalityFolder.SaveEegElectrodesFile(filePath, electrodes);

        public static void SaveEegCoordinateSystemFile(string filePath, EegCoordinateSystem eegCoordinateSystem)
            => EegModalityFolder.SaveEegCoordinateSystemFile(filePath, eegCoordinateSystem);

        public static void SaveTaskEventsFile(string filePath, TaskEventCollection taskEvents)
            => EegModalityFolder.SaveTaskEventsFile(filePath, taskEvents);
        #endregion
    }
}
