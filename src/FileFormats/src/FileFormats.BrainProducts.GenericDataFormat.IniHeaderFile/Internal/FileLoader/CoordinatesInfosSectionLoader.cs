using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat.Properties;

namespace BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat.Internal
{
    internal static class CoordinatesInfosSectionLoader
    {
        public static bool TryProcess(HeaderFileContent content, string keyName, string keyValue, [NotNullWhen(false)] out string? exceptionMessage)
        {
            List<Coordinates>? channelCoordinates = content.GetChannelCoordinates()!;

            if (!FileLoaderCommon.TryParseChannelNumber(keyName, out int channelNumber))
            {
                exceptionMessage = $"{Resources.UnrecognizedKey} {keyName}";
                return false;
            }

            if (channelNumber != channelCoordinates.Count + 1)
            {
                exceptionMessage = $"{Resources.NonConsecutiveChannelNumber} {keyName}";
                return false;
            }

            if (channelNumber <= 0)
            {
                exceptionMessage = $"{Resources.UnrecognizedKey} {keyName}";
                return false;
            }

            bool success = TryParseCoordinates(keyValue, out Coordinates coordinates, out string? errorText);
            if (!success)
            {
                exceptionMessage = $"{errorText} {Resources.Channel} {keyName}";
                return false;
            }

            channelCoordinates.Add(coordinates);

            exceptionMessage = null;
            return true;
        }

        private static bool TryParseCoordinates(string s, out Coordinates coordinates, [NotNullWhen(false)] out string? exceptionMessage)
        {
            coordinates = new Coordinates();

            string[] items = s.Split(',');

            if (items.Length < 3)
            {
                exceptionMessage = Resources.CoordinatesIncomplete;
                return false;
            }

            for (int i = 0; i < items.Length; ++i)
            {
                string item = items[i];

                if (!double.TryParse(item, NumberStyles.Float, CultureInfo.InvariantCulture, out double val))
                {
                    exceptionMessage = Resources.CoordinatesCannotBeParsed;
                    return false;
                }

                switch (i)
                {
                    case 0:
                        coordinates.Radius = val;
                        break;
                    case 1:
                        coordinates.Theta = val;
                        break;
                    case 2:
                        coordinates.Phi = val;
                        break;
                    default:
                        {
                            exceptionMessage = Resources.CoordinatesWithSurplusItems;
                            return false;
                        }
                }
            }

            exceptionMessage = null;
            return true;
        }
    }
}
