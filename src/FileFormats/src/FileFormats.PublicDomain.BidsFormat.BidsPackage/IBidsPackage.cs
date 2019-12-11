namespace BrainVision.Lab.FileFormats.PublicDomain.BidsFormat
{
    public interface IBidsPackage
    {
        IRootFolder RootFolder { get; }

        string Subject { get; }
        string? Session { get; }
        string Task { get; }

        /// <summary>
        /// File name pattern, files named according to this pattern are supposed to belong to the BIDS package
        /// </summary>
        string FileNamePattern { get; }
    }
}
