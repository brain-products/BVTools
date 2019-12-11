using System;
using System.Diagnostics;

namespace BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat.Internal
{
    internal static class InlinedCommentsLoader
    {
        public static void ProcessSectionComment(HeaderFileContent content, Definitions.Section section, string comment)
        {
            switch (section)
            {
                case Definitions.Section.NoSection:
                    content.InlinedComments.BelowHeaderSection = FileLoaderCommon.ConcatenateWithNewLine(content.InlinedComments.BelowHeaderSection, comment);
                    break;

                case Definitions.Section.CommonInfos:
                    content.InlinedComments.BelowCommonInfosSection = FileLoaderCommon.ConcatenateWithNewLine(content.InlinedComments.BelowCommonInfosSection, comment);
                    break;

                case Definitions.Section.BinaryInfos:
                    content.InlinedComments.BelowBinaryInfosSection = FileLoaderCommon.ConcatenateWithNewLine(content.InlinedComments.BelowBinaryInfosSection, comment);
                    break;

                case Definitions.Section.ChannelInfos:
                    content.InlinedComments.BelowChannelInfosSection = FileLoaderCommon.ConcatenateWithNewLine(content.InlinedComments.BelowChannelInfosSection, comment);
                    break;

                case Definitions.Section.Coordinates:
                    content.InlinedComments.BelowCoordinatesInfosSection = FileLoaderCommon.ConcatenateWithNewLine(content.InlinedComments.BelowCoordinatesInfosSection, comment);
                    break;

                case Definitions.Section.Comment:
                    content.InlinedComments.BelowCommentSection = FileLoaderCommon.ConcatenateWithNewLine(content.InlinedComments.BelowCommentSection, comment);
                    break;

                default:
                    throw new NotImplementedException(); // should never happen
            }
        }

        public static void ProcessKeyComment(HeaderFileContent content, Definitions.Section section, string keyName, string comment)
        {
            switch (section)
            {
                case Definitions.Section.CommonInfos:
                    if (keyName == Definitions.CommonInfosKeys.DataOrientation.ToString())
                        content.InlinedComments.AboveDataOrientation = FileLoaderCommon.ConcatenateWithNewLine(content.InlinedComments.AboveDataOrientation, comment);
                    else if (keyName == Definitions.CommonInfosKeys.SamplingInterval.ToString())
                        content.InlinedComments.AboveSamplingInterval = FileLoaderCommon.ConcatenateWithNewLine(content.InlinedComments.AboveSamplingInterval, comment);
                    else
                        Debug.Fail("Unhandled comment");
                    break;

                case Definitions.Section.NoSection:
                case Definitions.Section.BinaryInfos:
                case Definitions.Section.ChannelInfos:
                case Definitions.Section.Coordinates:
                case Definitions.Section.Comment:
                    Debug.Fail("Unhandled comment");
                    break;

                default:
                    throw new NotImplementedException(); // should never happen
            }
        }
    }
}
