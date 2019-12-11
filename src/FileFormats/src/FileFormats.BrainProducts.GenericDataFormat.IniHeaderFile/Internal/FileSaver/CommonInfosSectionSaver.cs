using System;
using System.Globalization;
using System.IO;

namespace BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat.Internal
{
    internal static class CommonInfosSectionSaver
    {
        public static void Save(StreamWriter writer, IHeaderFileContentVer1 content)
        {
            writer.WriteLine();
            writer.WriteLine(IniFormat.FormatSectionName(Definitions.GetSectionName(Definitions.Section.CommonInfos)!));
            FileSaverCommon.WriteCommentBlock(writer, content.InlinedComments.BelowCommonInfosSection);

            Definitions.CommonInfosKeys[] commonInfosKeys = (Definitions.CommonInfosKeys[])Enum.GetValues(typeof(Definitions.CommonInfosKeys));

            foreach (Definitions.CommonInfosKeys key in commonInfosKeys)
            {
                string? keyValue;

                switch (key)
                {
                    case Definitions.CommonInfosKeys.Codepage:
                        // replacing Utf8 enum with Utf-8 string
                        keyValue = (content.CodePage == Codepage.Utf8) ? Definitions.Utf8Enum : content.CodePage?.ToString();
                        break;

                    case Definitions.CommonInfosKeys.DataFile:
                        keyValue = content.DataFile;
                        break;

                    case Definitions.CommonInfosKeys.MarkerFile:
                        keyValue = content.MarkerFile;
                        break;

                    case Definitions.CommonInfosKeys.DataFormat:
                        keyValue = content.DataFormat?.ToString();
                        break;

                    case Definitions.CommonInfosKeys.DataType:
                        keyValue = content.DataType?.ToString();
                        break;

                    case Definitions.CommonInfosKeys.DataOrientation:
                        if (content.DataOrientation != null)
                            FileSaverCommon.WriteCommentBlock(writer, content.InlinedComments.AboveDataOrientation);
                        keyValue = content.DataOrientation?.ToString();
                        break;

                    case Definitions.CommonInfosKeys.SamplingInterval:
                        if (content.SamplingInterval != null)
                            FileSaverCommon.WriteCommentBlock(writer, content.InlinedComments.AboveSamplingInterval);
                        keyValue = content.SamplingInterval?.ToString("R", CultureInfo.InvariantCulture);
                        break;

                    case Definitions.CommonInfosKeys.NumberOfChannels:
                        keyValue = content.NumberOfChannels?.ToString(CultureInfo.InvariantCulture);
                        break;

                    case Definitions.CommonInfosKeys.Averaged:
                        keyValue = content.Averaged.HasValue ?
                            content.Averaged.Value ? YesNo.YES.ToString() : YesNo.NO.ToString() :
                            null;
                        break;

                    case Definitions.CommonInfosKeys.AveragedSegments:
                        keyValue = content.AveragedSegments?.ToString(CultureInfo.InvariantCulture);
                        break;

                    case Definitions.CommonInfosKeys.SegmentDataPoints:
                        keyValue = content.SegmentDataPoints?.ToString(CultureInfo.InvariantCulture);
                        break;

                    case Definitions.CommonInfosKeys.SegmentationType:
                        keyValue = content.SegmentationType?.ToString();
                        break;

                    default:
                        throw new NotImplementedException(); // should never happen
                }

                if (keyValue != null)
                {
                    string line = IniFormat.FormatKeyValueLine(key.ToString(), keyValue);
                    writer.WriteLine(line);
                }
            }
        }
    }
}
