using System.Globalization;

namespace BrainVision.Lab.FileFormats.Internal.Converters;

internal static class ChangesConverter
{
    public static string Collect() => $"1.0.0 {DateTime.Now.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}{Environment.NewLine} - Initial release.";
}
