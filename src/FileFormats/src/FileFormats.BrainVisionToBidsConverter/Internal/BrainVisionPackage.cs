using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat;
using BrainVision.Lab.FileFormats.Properties;

namespace BrainVision.Lab.FileFormats.Internal
{
    /// <summary>
    /// Package keeps paths and content of all Brain Vision files in one place.
    /// </summary>
    internal class BrainVisionPackage
    {
        private string _rawDataFilePath;
        private string _headerFilePath;
        private string? _markerFilePath;

        public string RawDataFilePath => _rawDataFilePath;
        public string HeaderFilePath => _headerFilePath;
        public string? MarkerFilePath => _markerFilePath;

        public IHeaderFileContentVer1 HeaderFileContent { get; set; }
        public IMarkerFileContentVer1? MarkerFileContent { get; set; }

        private BrainVisionPackage(string brainVisionHeaderFilePath)
        {
            string headerFullPath = Path.GetFullPath(brainVisionHeaderFilePath);
            HeaderFileContent = LoadHeaderFile(headerFullPath, out _headerFilePath);

            string? markerFullPath = GetMarkerFileFullPath(headerFullPath, HeaderFileContent.MarkerFile);
            if (markerFullPath != null)
                MarkerFileContent = LoadMarkerFile(markerFullPath, out _markerFilePath);

            string dataFullPath = GetDataFileFullPath(headerFullPath, HeaderFileContent.DataFile!);
            LoadRawDataFile(dataFullPath, out _rawDataFilePath);
        }

        public static BrainVisionPackage Copy(BrainVisionPackage srcPackage)
            => (BrainVisionPackage)srcPackage.MemberwiseClone();

        public static BrainVisionPackage Load(string brainVisionHeaderFilePath)
        {
            BrainVisionPackage brainVisionFilesContent = new BrainVisionPackage(brainVisionHeaderFilePath);
            return brainVisionFilesContent;
        }

        private static IHeaderFileContentVer1 LoadHeaderFile(string path, out string headerFilePath)
        {
            try
            {
                using IHeaderFile headerFile = HeaderFileFactory.OpenForRead(path);
                headerFilePath = path;
                return headerFile.LoadVer1();
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception e)
            {
                throw new InvalidOperationException($"{Resources.FailedToLoadBrainVisionHeaderFileExceptionMessage} {e.Message}", e);
            }
#pragma warning restore CA1031 // Do not catch general exception types
        }

        private static IMarkerFileContentVer1 LoadMarkerFile(string path, out string markerFilePath)
        {
            try
            {
                using IMarkerFile markerFile = MarkerFileFactory.OpenForRead(path);
                markerFilePath = path;
                return markerFile.LoadVer1();
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception e)
            {
                throw new InvalidOperationException($"{Resources.FailedToLoadBrainVisionMarkerFileExceptionMessage} {e.Message}", e);
            }
#pragma warning restore CA1031 // Do not catch general exception types
        }

        private static void LoadRawDataFile(string path, out string rawDataFilePath)
        {
            if (!File.Exists(path))
                throw new InvalidOperationException($"{Resources.FailedToFindBrainVisionRawDataFileExceptionMessage} {path}");

            rawDataFilePath = path;
        }

        private static string GetDataFileFullPath(string headerFullPath, string dataFileName)
        {
            string dataFullPath = Path.Combine(Path.GetPathRoot(headerFullPath), Path.GetDirectoryName(headerFullPath), dataFileName);
            return dataFullPath;
        }

        [return: NotNullIfNotNull("markerFileName")]
        private static string? GetMarkerFileFullPath(string headerFullPath, string? markerFileName)
        {
            if (markerFileName == null)
                return null;

            string markerFullPath = Path.Combine(Path.GetPathRoot(headerFullPath), Path.GetDirectoryName(headerFullPath), markerFileName);
            return markerFullPath;
        }

        public void Save(string brainVisionHeaderFilePath)
        {
            string headerFullPath = Path.GetFullPath(brainVisionHeaderFilePath);
            using (IHeaderFile headerFile = HeaderFileFactory.OpenForReadWrite(headerFullPath))
            {
                headerFile.SaveVer1(HeaderFileContent);
            }

            if (MarkerFileContent != null)
            {
                string markerFullPath = GetMarkerFileFullPath(headerFullPath, HeaderFileContent.MarkerFile!);
                using IMarkerFile markerFile = MarkerFileFactory.OpenForReadWrite(markerFullPath);
                markerFile.SaveVer1(MarkerFileContent);
            }
        }

        public void UpdateMissingKeysWithDefaultValues()
            => HeaderFileContent.UpdateMissingKeysWithDefaultValues();

        public void SetPathsAndUpdateFileReferencesInBvFiles(string rawDataFilePath, string headerFilePath, string markerFilePath)
        {
            _rawDataFilePath = rawDataFilePath;
            _headerFilePath = headerFilePath;
            _markerFilePath = markerFilePath;

            string rawDataFileFileName = Path.GetFileName(_rawDataFilePath);
            string markerFileFileName = Path.GetFileName(_markerFilePath);

            UpdateFileReferencesInBvFiles(rawDataFileFileName, markerFileFileName);
        }

        public void UpdateFileReferencesInBvFiles(string rawDataFileFileName, string markerFileFileName)
        {
            HeaderFileContent.DataFile = rawDataFileFileName;
            HeaderFileContent.MarkerFile = markerFileFileName;
            if (MarkerFileContent != null)
                MarkerFileContent.DataFile = rawDataFileFileName;
        }
    }
}
