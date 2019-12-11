using System;
using System.Diagnostics;

namespace BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat.Internal
{
    internal static class InlinedCommentsLoader
    {
        public static void ProcessSectionComment(MarkerFileContent content, Definitions.Section section, string comment)
        {
            switch (section)
            {
                case Definitions.Section.NoSection:
                    content.InlinedComments.BelowHeaderSection = FileLoaderCommon.ConcatenateWithNewLine(content.InlinedComments.BelowHeaderSection, comment);
                    break;

                case Definitions.Section.CommonInfos:
                    content.InlinedComments.BelowCommonInfosSection = FileLoaderCommon.ConcatenateWithNewLine(content.InlinedComments.BelowCommonInfosSection, comment);
                    break;

                case Definitions.Section.MarkerInfos:
                    content.InlinedComments.BelowMarkerInfosSection = FileLoaderCommon.ConcatenateWithNewLine(content.InlinedComments.BelowMarkerInfosSection, comment);
                    break;

                default:
                    throw new NotImplementedException(); // should never happen
            }
        }

        // actually no key comments to process
        public static void ProcessKeyComments(Definitions.Section section)//, string keyName, string keyEntryComments)
        {
            switch (section)
            {
                case Definitions.Section.NoSection:
                case Definitions.Section.CommonInfos:
                case Definitions.Section.MarkerInfos:
                    Debug.Fail("Unhandled comment");
                    break;

                default:
                    throw new NotImplementedException(); // should never happen
            }
        }
    }
}
