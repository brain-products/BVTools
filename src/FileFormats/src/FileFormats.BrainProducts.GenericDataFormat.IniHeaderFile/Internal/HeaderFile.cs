using System;
using System.IO;
using BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat.Properties;

namespace BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat.Internal
{
    internal class HeaderFile : IHeaderFile
    {
        #region variables
        private readonly FileStream _file;
        private readonly bool _newFileBeingCreated;
        #endregion

        public HeaderFile(string path, FileMode mode, FileAccess access, FileShare share)
        {
            bool openingExistingFile = File.Exists(path);

            _file = new FileStream(path, mode, access, share);

            _newFileBeingCreated = !openingExistingFile || mode == FileMode.Truncate;
            if (_newFileBeingCreated)
            {
                Version = new Version(1, 0);

                FileSaver saver = new FileSaver(_file);
                saver.SaveEmpty();
            }
            else
            {
                ThrowExceptionIfFileExtensionNotRecognized(path);

                FileLoader fileLoader = new FileLoader(_file);
                Version = fileLoader.ReadVersion();
            }
        }

        private void ThrowExceptionIfFileExtensionNotRecognized(string path)
        {
            string fileExtension = Path.GetExtension(path);
            const string validExtension = "vhdr";
            bool isExtensionValid = fileExtension == $".{validExtension}";
            if (!isExtensionValid)
                throw new InvalidHeaderFileFormatException(0, $"{Resources.UnrecognizedFileExtension} {validExtension}");
        }

        #region load/save
        public Version Version { get; }

        public IHeaderFileContentVer1 LoadVer1()
        {
            if (_newFileBeingCreated)
            {
                HeaderFileContent content = new HeaderFileContent(Definitions.IdentificationText, new Version(1, 0));
                return content;
            }
            else
            {
                FileLoader fileLoader = new FileLoader(_file);
                return fileLoader.LoadVer1();
            }
        }

        public void SaveVer1(IHeaderFileContentVer1 header)
        {
            FileSaver fileSaver = new FileSaver(_file);
            fileSaver.SaveVer1(header);
        }
        #endregion

        #region dispose
        public void Dispose() => _file.Dispose();
        #endregion
    }
}
