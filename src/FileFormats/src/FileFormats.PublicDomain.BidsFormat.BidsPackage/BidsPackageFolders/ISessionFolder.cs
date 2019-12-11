namespace BrainVision.Lab.FileFormats.PublicDomain.BidsFormat
{
    public interface ISessionFolder
    {
        #region Properties
        ISubjectFolder Parent { get; }
        IEegModalityFolder EegModalityFolder { get; }
        string? FolderName { get; }
        string FolderPath { get; }
        #endregion
    }
}
