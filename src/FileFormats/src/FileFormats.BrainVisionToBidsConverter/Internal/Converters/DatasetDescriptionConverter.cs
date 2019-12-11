using BrainVision.Lab.FileFormats.PublicDomain.BidsFormat;

namespace BrainVision.Lab.FileFormats.Internal.Converters
{
    internal static class DatasetDescriptionConverter
    {
        public static DatasetDescription Collect(CustomizationInfo info)
        {
            DatasetDescription description = new DatasetDescription(
                // REQUIRED
                name: info.DatasetName)
                //BIDSVersion, // set internally in constructor
            {
                // RECOMMENDED
                License = info.License,
                // OPTIONAL
                Authors = info.Authors,
            };

            return description;
        }
    }
}
