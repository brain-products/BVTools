using System;
using System.IO;

namespace BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat.Internal
{
    internal class FileSaver
    {
        private readonly StreamWriter _writer;

        public FileSaver(FileStream file)
        {
            _writer = new StreamWriter(file); // writer is never explicitly disposed to avoid closing the underlying stream
        }

        public void SaveEmpty()
        {
            _writer.BaseStream.SetLength(0);
            _writer.WriteLine(Definitions.IdentificationText + new Version(1, 0).ToString());
            _writer.Flush();
        }

        public void SaveVer1(IHeaderFileContentVer1 content)
        {
            _writer.BaseStream.SetLength(0);

            SaveFileHeader(content);
            CommonInfosSectionSaver.Save(_writer, content);
            BinaryInfosSectionSaver.Save(_writer, content);
            ChannelInfosSectionSaver.Save(_writer, content);
            CoordinatesSectionSaver.Save(_writer, content);
            CommentSectionSaver.Save(_writer, content);

            _writer.Flush();
        }

        private void SaveFileHeader(IHeaderFileContentVer1 content)
        {
            _writer.WriteLine(content.IdentificationText);
            FileSaverCommon.WriteCommentBlock(_writer, content.InlinedComments.BelowHeaderSection);
        }
    }
}
