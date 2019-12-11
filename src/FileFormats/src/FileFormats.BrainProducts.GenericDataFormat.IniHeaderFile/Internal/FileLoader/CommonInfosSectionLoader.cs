using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat.Properties;

namespace BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat.Internal
{
    internal static class CommonInfosSectionLoader
    {
        private readonly static string[] s_validEegFileExtensions = new string[] { "eeg", "avg", "seg" };
        private readonly static string[] s_validMarkerFileExtensions = new string[] { "vmrk" };

        public static bool TryProcess(HeaderFileContent content, string keyName, string keyValue, [NotNullWhen(false)] out string? exceptionMessage)
        {
            if (!Enum.TryParse(keyName, true, out Definitions.CommonInfosKeys key))
            {
                exceptionMessage = $"{Resources.UnrecognizedKey} {keyName}";
                return false;
            }

            switch (key)
            {
                case Definitions.CommonInfosKeys.Codepage:
                    {
                        // resolving Utf-8 string as Utf8 enum
                        if (0 == string.Compare(keyValue, Definitions.Utf8Enum, true, CultureInfo.InvariantCulture))
                        {
                            content.CodePage = Codepage.Utf8;
                        }
                        else
                        {
                            if (!Enum.TryParse(keyValue, true, out Codepage value))
                            {
                                exceptionMessage = $"{Resources.UnrecognizedKeyValue} {keyValue}";
                                return false;
                            }

                            // Utf8 is not valid, only Utf-8 is valid
                            if (value == Codepage.Utf8)
                            {
                                exceptionMessage = $"{Resources.UnrecognizedKeyValue} {keyValue}";
                                return false;
                            }

                            content.CodePage = value;
                        }
                    }
                    break;

                case Definitions.CommonInfosKeys.DataFile:
                    if (string.IsNullOrWhiteSpace(keyValue))
                    {
                        exceptionMessage = $"{Resources.FileNameCannotBeEmpty} {keyName}";
                        return false;
                    }

                    string eegFileExtension = Path.GetExtension(keyValue);
                    bool isEegFileExtensionValid = s_validEegFileExtensions.Select(p => $".{p}").Contains(eegFileExtension);
                    if (!isEegFileExtensionValid)
                    {
                        exceptionMessage = $"{Resources.UnrecognizedFileExtension} {ToCommaSeparatedText(s_validEegFileExtensions)}: {keyName}";
                        return false;
                    }
                    content.DataFile = keyValue;
                    break;

                case Definitions.CommonInfosKeys.MarkerFile:
                    if (string.IsNullOrWhiteSpace(keyValue))
                    {
                        exceptionMessage = $"{Resources.FileNameCannotBeEmpty} {keyName}";
                        return false;
                    }

                    string markerFileExtension = Path.GetExtension(keyValue);
                    bool isMarkerFileExtensionValid = s_validMarkerFileExtensions.Select(p => $".{p}").Contains(markerFileExtension);
                    if (!isMarkerFileExtensionValid)
                    {
                        exceptionMessage = $"{Resources.UnrecognizedFileExtension} {ToCommaSeparatedText(s_validMarkerFileExtensions)}: {keyName}";
                        return false;
                    }
                    content.MarkerFile = keyValue;
                    break;

                case Definitions.CommonInfosKeys.DataFormat:
                    {
                        if (!Enum.TryParse(keyValue, true, out DataFormat value))
                        {
                            exceptionMessage = $"{Resources.UnrecognizedKeyValue} {keyValue}";
                            return false;
                        }

                        content.DataFormat = value;
                    }
                    break;

                case Definitions.CommonInfosKeys.DataType:
                    {
                        if (!Enum.TryParse(keyValue, true, out DataType value))
                        {
                            exceptionMessage = $"{Resources.UnrecognizedKeyValue} {keyValue}";
                            return false;
                        }

                        content.DataType = value;
                    }
                    break;

                case Definitions.CommonInfosKeys.DataOrientation:
                    {
                        if (!Enum.TryParse(keyValue, true, out DataOrientation value))
                        {
                            exceptionMessage = $"{Resources.UnrecognizedKeyValue} {keyValue}";
                            return false;
                        }

                        content.DataOrientation = value;
                    }
                    break;

                case Definitions.CommonInfosKeys.SamplingInterval:
                    {
                        if (!double.TryParse(keyValue, NumberStyles.Float, CultureInfo.InvariantCulture, out double value))
                        {
                            exceptionMessage = $"{Resources.UnrecognizedKeyValue} {keyValue}";
                            return false;
                        }

                        if (value <= 0.0)
                        {
                            exceptionMessage = $"{Resources.ValueCannotBeNegativeOrZero} {keyName}";
                            return false;
                        }

                        content.SamplingInterval = value;
                    }
                    break;

                case Definitions.CommonInfosKeys.NumberOfChannels:
                    {
                        if (!int.TryParse(keyValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out int value))
                        {
                            exceptionMessage = $"{Resources.UnrecognizedKeyValue} {keyValue}";
                            return false;
                        }

                        if (value <= 0)
                        {
                            exceptionMessage = $"{Resources.ValueCannotBeNegativeOrZero} {keyName}";
                            return false;
                        }

                        content.NumberOfChannels = value;
                    }
                    break;

                case Definitions.CommonInfosKeys.Averaged:
                    {
                        if (!Enum.TryParse(keyValue, true, out YesNo value))
                        {
                            exceptionMessage = $"{Resources.UnrecognizedKeyValue} {keyValue}";
                            return false;
                        }

                        content.Averaged = value == YesNo.YES;
                    }
                    break;

                case Definitions.CommonInfosKeys.AveragedSegments:
                    {
                        if (!int.TryParse(keyValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out int value))
                        {
                            exceptionMessage = $"{Resources.UnrecognizedKeyValue} {keyValue}";
                            return false;
                        }

                        if (value < 0)
                        {
                            exceptionMessage = $"{Resources.ValueCannotBeNegative} {keyName}";
                            return false;
                        }

                        content.AveragedSegments = value;
                    }
                    break;

                case Definitions.CommonInfosKeys.SegmentDataPoints:
                    {
                        if (!int.TryParse(keyValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out int value))
                        {
                            exceptionMessage = $"{Resources.UnrecognizedKeyValue} {keyValue}";
                            return false;
                        }

                        if (value < 0)
                        {
                            exceptionMessage = $"{Resources.ValueCannotBeNegative} {keyName}";
                            return false;
                        }

                        content.SegmentDataPoints = value;
                    }
                    break;

                case Definitions.CommonInfosKeys.SegmentationType:
                    {
                        if (!Enum.TryParse(keyValue, true, out SegmentationType value))
                        {
                            exceptionMessage = $"{Resources.UnrecognizedKeyValue} {keyValue}";
                            return false;
                        }

                        content.SegmentationType = value;
                    }
                    break;

                default:
                    throw new NotImplementedException(); // should never happen
            }

            exceptionMessage = null;
            return true;
        }

        private static string ToCommaSeparatedText(string[] texts)
        {
            StringBuilder sb = new StringBuilder();
            Array.ForEach(texts, p => sb.Append(sb.Length == 0 ? string.Empty : ", ").Append(p));
            return sb.ToString();
        }
    }
}
