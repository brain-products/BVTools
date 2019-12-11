using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat.Properties;

namespace BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat.Internal
{
    internal class ChannelInfosSectionLoader
    {
        public static bool TryProcess(HeaderFileContent content, string keyName, string keyValue, [NotNullWhen(false)] out string? exceptionMessage)
        {
            List<ChannelInfo> channelInfos = content.GetChannelInfos()!;

            if (!FileLoaderCommon.TryParseChannelNumber(keyName, out int channelNumber))
            {
                exceptionMessage = $"{Resources.UnrecognizedKey} {keyName}";
                return false;
            }

            if (channelNumber != channelInfos.Count + 1)
            {
                exceptionMessage = $"{Resources.NonConsecutiveChannelNumber} {keyName}";
                return false;
            }

            bool success = TryParseChannelInfo(keyValue, out ChannelInfo channelInfo, out string? errorText);
            if (!success)
            {
                exceptionMessage = $"{errorText} {Resources.Channel} {keyName}";
                return false;
            }

            channelInfos.Add(channelInfo);

            exceptionMessage = null;
            return true;
        }

        private static bool TryParseChannelInfo(string s, out ChannelInfo channelInfo, [NotNullWhen(false)] out string? exceptionMessage)
        {
            channelInfo = new ChannelInfo();

            string[] items = s.Split(',');

            if (items.Length < 3) // obligatory items
            {
                exceptionMessage = Resources.ChannelInfoIncomplete;
                return false;
            }

            for (int i = 0; i < items.Length; ++i)
            {
                string item = items[i];
                switch (i)
                {
                    case 0:
                        if (string.IsNullOrEmpty(item))
                        {
                            exceptionMessage = $"{Resources.ValueCannotBeEmpty} Channel Name.";
                            return false;
                        }

                        channelInfo.Name = item.Replace(Definitions.PlaceholderForCommaChar, ',');
                        break;

                    case 1:
                        channelInfo.RefName = item.Replace(Definitions.PlaceholderForCommaChar, ',');
                        break;

                    case 2:
                        {
                            if (string.IsNullOrWhiteSpace(item))
                            {
                                channelInfo.Resolution = null;
                            }
                            else if (!double.TryParse(item, NumberStyles.Float, CultureInfo.InvariantCulture, out double val))
                            {
                                exceptionMessage = $"{Resources.ValueCannotBeParsed} Resolution.";
                                return false;
                            }
                            else if (val <= 0.0)
                            {
                                exceptionMessage = $"{Resources.ValueCannotBeNegativeOrZero} Resolution.";
                                return false;
                            }
                            else
                            {
                                channelInfo.Resolution = val;
                            }
                        }
                        break;

                    case 3: // optional
                        channelInfo.Unit = item;
                        break;

                    default:
                        {
                            exceptionMessage = Resources.ChannelInfoWithSurplusItems;
                            return false;
                        }
                }
            }

            exceptionMessage = null;
            return true;
        }
    }
}
