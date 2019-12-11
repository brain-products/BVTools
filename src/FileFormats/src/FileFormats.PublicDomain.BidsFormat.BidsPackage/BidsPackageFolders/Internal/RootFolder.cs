using System.IO;
using BrainVision.Lab.FileFormats.PublicDomain.BidsFormat.Internal.Writers;

namespace BrainVision.Lab.FileFormats.PublicDomain.BidsFormat.Internal
{
    internal class RootFolder : IRootFolder
    {
        private readonly DatasetInfo _datasetInfo;
        internal RootFolder(DatasetInfo datasetInfo)
        {
            _datasetInfo = datasetInfo;
            SubjectFolder = new SubjectFolder(this, datasetInfo);
        }

        #region Properties
        public string FolderPath => _datasetInfo.Root;
        public ISubjectFolder SubjectFolder { get; }
        #endregion

        #region File Names
        public string DatasetDescriptionFileName { get; } = "dataset_description.json";
        public string ReadmeFileName { get; } = "README";
        public string ChangesFileName { get; } = "CHANGES";
        #endregion

        #region File Paths
        public string DatasetDescriptionFilePath => Path.Combine(FolderPath, DatasetDescriptionFileName);
        public string ReadmeFilePath => Path.Combine(FolderPath, ReadmeFileName);
        public string ChangesFilePath => Path.Combine(FolderPath, ChangesFileName);
        #endregion

        #region Save
        public void SaveDatasetDescriptionFile(DatasetDescription datasetDescription)
            => SaveDatasetDescriptionFile(DatasetDescriptionFilePath, datasetDescription);

        public void SaveReadmeFile(string fileContent)
            => SaveReadmeFile(ReadmeFilePath, fileContent);

        public void SaveChangesFile(string fileContent)
            => SaveChangesFile(ChangesFilePath, fileContent);
        #endregion

        #region Static Save
        public static void SaveDatasetDescriptionFile(string filePath, DatasetDescription datasetDescription)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            DatasetDescriptionWriter.Save(filePath, datasetDescription);
        }

        public static void SaveReadmeFile(string filePath, string fileContent)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            ReadmeWriter.Save(filePath, fileContent);
        }

        public static void SaveChangesFile(string filePath, string fileContent)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            ChangesWriter.Save(filePath, fileContent);
        }
        #endregion
    }
}
