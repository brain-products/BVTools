namespace BrainVision.Lab.FileFormats.PublicDomain.BidsFormat
{
    public interface ISubjectFolder
    {
        #region Properties
        IRootFolder Parent { get; }
        ISessionFolder SessionFolder { get; }
        string FolderName { get; }
        string FolderPath { get; }
        #endregion
    }
}
