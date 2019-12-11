using System.IO;

namespace BrainVision.Lab.FileFormats.PublicDomain.BidsFormat.Internal
{
    internal class SubjectFolder : ISubjectFolder
    {
        private readonly DatasetInfo _datasetInfo;
        public const string FolderPrefix = "sub-";

        internal SubjectFolder(RootFolder parent, DatasetInfo datasetInfo)
        {
            _datasetInfo = datasetInfo;
            Parent = parent;
            SessionFolder = new SessionFolder(this, datasetInfo);
        }

        #region Properties
        public IRootFolder Parent { get; }
        public ISessionFolder SessionFolder { get; }
        public string FolderName => $"{FolderPrefix}{_datasetInfo.Subject}";
        public string FolderPath => Path.Combine(Parent.FolderPath, FolderName);
        #endregion
    }
}
