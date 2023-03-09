using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat.Properties;

namespace BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat.Internal;

internal static class MarkerInfosSectionLoader
{
    public static bool TryProcess(MarkerFileContent content, string keyName, string keyValue, [NotNullWhen(false)] out string? exceptionMessage)
    {
        IList<MarkerInfo> markers = content.GetMarkers()!;

        if (!TryParseMarkerNumber(keyName, out int markerNumber))
        {
            exceptionMessage = $"{Resources.UnrecognizedKey} {keyName}";
            return false;
        }

        if (markerNumber != markers.Count + 1)
        {
            exceptionMessage = $"{Resources.NonConsecutiveMarkerNumber} {keyName}";
            return false;
        }

        if (!TryParseMarkerInfo(keyValue, out MarkerInfo markerInfo, out string? errorText))
        {
            exceptionMessage = $"{errorText} {Resources.Marker} {keyName}";
            return false;
        }

        markers.Add(markerInfo);

        exceptionMessage = null;
        return true;
    }

    private static bool TryParseMarkerNumber(string keyName, out int markerNumber)
    {
        if (keyName.StartsWith(Definitions.KeyMkPlaceholder, true, CultureInfo.InvariantCulture))
        {
            string textExpectedToContainMarkerNumber = keyName[Definitions.KeyMkPlaceholder.Length..];
            if (int.TryParse(textExpectedToContainMarkerNumber, out markerNumber))
                return true;
        }

        markerNumber = -1;
        return false;
    }

    private static bool TryParseMarkerInfo(string s, out MarkerInfo markerInfo, [NotNullWhen(false)] out string? exceptionMessage)
    {
        markerInfo = new MarkerInfo();

        // if an obligatory field does not exist, it is set to some invalid value.
        // It means that not existing fields and invalid fields will result to the same invalid value and are NOT distinguishable.
        string[] items = s.Split(',');

        if (items.Length < 5) // obligatory items
        {
            exceptionMessage = Resources.MarkerInfoIncomplete;
            return false;
        }

        for (int i = 0; i < items.Length; ++i)
        {
            string item = items[i];
            switch (i)
            {
                case 0:
                    markerInfo.Type = item.Replace(Definitions.PlaceholderForCommaChar, ',');
                    break;

                case 1:
                    markerInfo.Description = item.Replace(Definitions.PlaceholderForCommaChar, ',');
                    break;

                case 2:
                    {
                        // Positions are stored in file as 1-indexed
                        if (!int.TryParse(item, NumberStyles.Integer, CultureInfo.InvariantCulture, out int val))
                        {
                            exceptionMessage = $"{Resources.ValueCannotBeParsed} Position.";
                            return false;
                        }
                        else if (val <= 0)
                        {
                            exceptionMessage = $"{Resources.ValueCannotBeNegativeOrZero} Position.";
                            return false;
                        }
                        else
                        {
                            markerInfo.Position = val - 1;
                        }
                    }
                    break;

                case 3:
                    {
                        if (!int.TryParse(item, NumberStyles.Integer, CultureInfo.InvariantCulture, out int val))
                        {
                            exceptionMessage = $"{Resources.ValueCannotBeParsed} Length.";
                            return false;
                        }
                        else if (val < 0)
                        {
                            exceptionMessage = $"{Resources.ValueCannotBeNegative} Length.";
                            return false;
                        }
                        else
                        {
                            markerInfo.Length = val;
                        }
                    }
                    break;

                case 4:
                    {
                        // in file channel numbers are stored as 1-indexed
                        // in file zero means attached to all channels
                        if (!int.TryParse(item, NumberStyles.Integer, CultureInfo.InvariantCulture, out int val))
                        {
                            exceptionMessage = $"{Resources.ValueCannotBeParsed} Channel number.";
                            return false;
                        }
                        else if (val < 0)
                        {
                            exceptionMessage = $"{Resources.ValueCannotBeNegative} Channel number.";
                            return false;
                        }
                        else
                        {
                            markerInfo.ChannelNumber = val - 1;
                        }
                    }
                    break;

                case 5: // optional
                    if (!string.IsNullOrEmpty(item))
                    {
                        try
                        {
                            markerInfo.Date = DateTime.ParseExact(item, "yyyyMMddHHmmssffffff", CultureInfo.InvariantCulture);
                        }
                        catch (FormatException e)
                        {
                            exceptionMessage = $"{e.Message} Date.";
                            return false;
                        }
                    }
                    break;

                default:
                    exceptionMessage = Resources.MarkerInfoWithSurplusItems;
                    return false;
            }
        }

        exceptionMessage = null;
        return true;
    }
}
