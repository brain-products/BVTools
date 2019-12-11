namespace BrainVision.Lab.FileFormats.PublicDomain.BidsFormat
{
    public interface IEegModalityFolder
    {
        #region Properties
        ISessionFolder Parent { get; }
        string FolderName { get; }
        string FolderPath { get; }
        #endregion

        #region File Names
        string EegSidecarFileName { get; }
        string EegChannelsFileName { get; }
        string EegElectrodesFileName { get; }
        string EegCoordinatesFileName { get; }
        string TaskEventsFileName { get; }
        #endregion

        #region File Paths
        string EegSidecarFilePath { get; }
        string EegChannelsFilePath { get; }
        string EegElectrodesFilePath { get; }
        string EegCoordinatesFilePath { get; }
        string TaskEventsFilePath { get; }
        #endregion

        #region Save
        void SaveEegSidecarFile(EegSidecar sidecar);
        void SaveEegChannelsFile(EegChannelCollection channels);
        void SaveEegElectrodesFile(EegElectrodeCollection electrodes);
        void SaveEegCoordinateSystemFile(EegCoordinateSystem eegCoordinateSystem);
        void SaveTaskEventsFile(TaskEventCollection taskEvents);
        #endregion
    }
}
