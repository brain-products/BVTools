using System.IO;

namespace BrainVision.Lab.FileFormats.PublicDomain.BidsFormat.Internal
{
    internal class SessionFolder : ISessionFolder
    {
        private readonly DatasetInfo _datasetInfo;
        public const string FolderPrefix = "ses-";

        internal SessionFolder(SubjectFolder parent, DatasetInfo datasetInfo)
        {
            _datasetInfo = datasetInfo;
            Parent = parent;
            EegModalityFolder = new EegModalityFolder(this, datasetInfo);
        }

        #region Properties
        public ISubjectFolder Parent { get; }
        public IEegModalityFolder EegModalityFolder { get; }
        public string? FolderName => _datasetInfo.Session == null ? null : $"{FolderPrefix}{_datasetInfo.Session}";
        public string FolderPath => _datasetInfo.Session == null ? Parent.FolderPath : Path.Combine(Parent.FolderPath, FolderName);
        #endregion
    }
}
