namespace BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat
{
    public class HeaderFileInlinedComments
    {
        public string? BelowHeaderSection { get; set; }
        public string? BelowCommonInfosSection { get; set; }
        public string? BelowBinaryInfosSection { get; set; }
        public string? BelowChannelInfosSection { get; set; }
        public string? BelowCoordinatesInfosSection { get; set; }
        public string? BelowCommentSection { get; set; }
        public string? AboveDataOrientation { get; set; }
        public string? AboveSamplingInterval { get; set; }
    }
}
