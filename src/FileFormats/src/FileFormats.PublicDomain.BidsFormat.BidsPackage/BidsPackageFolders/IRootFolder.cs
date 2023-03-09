namespace BrainVision.Lab.FileFormats.PublicDomain.BidsFormat;

public interface IRootFolder
{
    #region Properties
    string FolderPath { get; }
    ISubjectFolder SubjectFolder { get; }
    #endregion

    #region File Names
    string DatasetDescriptionFileName { get; }
    string ReadmeFileName { get; }
    string ChangesFileName { get; }
    #endregion

    #region File Paths
    string DatasetDescriptionFilePath { get; }
    string ReadmeFilePath { get; }
    string ChangesFilePath { get; }
    #endregion

    #region Save
    Task SaveDatasetDescriptionFileAsync(DatasetDescription datasetDescription);

    Task SaveReadmeFileAsync(string fileContent);

    Task SaveChangesFileAsync(string fileContent);
    #endregion
}
