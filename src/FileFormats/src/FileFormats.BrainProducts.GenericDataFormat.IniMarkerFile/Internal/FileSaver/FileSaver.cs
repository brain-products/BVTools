namespace BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat.Internal;

internal sealed class FileSaver
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

    public async Task SaveVer1Async(IMarkerFileContentVer1 content)
    {
        _writer.BaseStream.SetLength(0);

        await SaveFileHeaderAsync(content).ConfigureAwait(false);

        await CommonInfosSectionSaver.SaveAsync(_writer, content).ConfigureAwait(false);
        await MarkerInfosSectionSaver.SaveAsync(_writer, content).ConfigureAwait(false);

        await _writer.FlushAsync().ConfigureAwait(false);
    }

    private async Task SaveFileHeaderAsync(IMarkerFileContentVer1 content)
    {
        await _writer.WriteLineAsync(content.IdentificationText).ConfigureAwait(false);
        await FileSaverCommon.WriteCommentBlockAsync(_writer, content.InlinedComments.BelowHeaderSection).ConfigureAwait(false);
    }
}
